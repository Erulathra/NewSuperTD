using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using NewSuperTD.Tiles.Scenes;
using NewSuperTD.Towers;
using NewSuperTD.Towers.Modifiers;

namespace NewSuperTD.Enemies;

public partial class Enemy : Node3D
{
	[Signal] public delegate void DeathEventHandler(Enemy enemy);

	[Signal] public delegate void FinishMovingEventHandler();

	[Signal] public delegate void ReachTargetEventHandler(Enemy enemy);

	private GlobalTickTimer globalTickTimer;

	[ExportGroup("Logic")]
	[Export] private int healthPoints = 100;

	private bool isGoingToDeath;

	[ExportGroup("Animation")]
	[Export] private float jumpHeight = 0.2f;

	[Export] private int moveTickCount = 10;

	private Tween moveTween;

	private Tile targetTile;
	[Export] private int thinkingTickCount = 20;

	public int HealthPoints
	{
		get => healthPoints;
		set
		{
			healthPoints = value;
			if (healthPoints <= 0 && !isGoingToDeath)
				OnDeath();
		}
	}

	public override void _Ready()
	{
		globalTickTimer = (GlobalTickTimer)GetTree().Root.FindChild("GlobalTickTimer", true, false);
		globalTickTimer.GlobalTick += OnGlobalTick;
	}

	public void StopThinking()
	{
		globalTickTimer.GlobalTick -= OnGlobalTick;
	}

	private void OnGlobalTick(int tickCount, GlobalTickTimer globalTickTimer)
	{
		if (tickCount % thinkingTickCount != 0)
			return;

		PathTile parentTile = GetParent<PathTile>();
		int distanceToKing = parentTile.DistanceToKing;

		Array<Tile> neighbors = parentTile.GetNeighbors();
		targetTile = neighbors.OfType<PathTile>().First(pathNeighbor => pathNeighbor.DistanceToKing < distanceToKing);

		if (targetTile is KingTile)
			EmitSignal(SignalName.ReachTarget, this);

		StartMoving();
	}

	private void StartMoving()
	{
		PathTile parentTile = GetParent<PathTile>();
		Vector3 parentPosition = parentTile.GetNode<Node3D>("SurfaceHandle").GlobalPosition;
		Vector3 targetPosition = targetTile.GetNode<Node3D>("SurfaceHandle").GlobalPosition;

		float distanceBetweenParentAndTarget = (parentPosition - targetPosition).Length();
		float tweenDuration = (float)globalTickTimer.WaitTime * moveTickCount;

		Callable moveCallable = Callable.From((Vector3 newPosition) => Move(newPosition, distanceBetweenParentAndTarget));
		moveTween = CreateTween();
		moveTween.SetTrans(Tween.TransitionType.Sine);
		moveTween.SetEase(Tween.EaseType.InOut);
		moveTween.TweenMethod(moveCallable, parentPosition, targetPosition, tweenDuration);
		moveTween.Parallel().TweenCallback(Callable.From(() =>
		{
			moveTween.Kill();
			EmitSignal(SignalName.FinishMoving);
		})).SetDelay(tweenDuration);

		Animate(targetPosition, tweenDuration);
	}

	private void Move(Vector3 position, float distanceBetweenParentAndTarget)
	{
		if (targetTile == null)
			return;

		GlobalPosition = position;

		float distanceToTarget = (GlobalPosition - targetTile.GlobalPosition).Length();
		if (distanceToTarget < 0.5 * distanceBetweenParentAndTarget)
			ChangeParentTile();
	}

	private void ChangeParentTile()
	{
		if (GetParent() == targetTile)
			return;

		ModifierHandler oldParentModifierHandler = GetParent<Tile>().GetNode<ModifierHandler>("ModifierHandler");
		oldParentModifierHandler.OnRegisterModifier -= OnParentRegisterModifier;

		Reparent(targetTile);

		ModifierHandler targetModifierHandler = targetTile.GetNode<ModifierHandler>("ModifierHandler");
		foreach (Modifier modifier in targetModifierHandler.GetCurrentModifiersArray())
			modifier.GetDamage(this);

		targetModifierHandler.OnRegisterModifier += OnParentRegisterModifier;
	}

	private void OnParentRegisterModifier(Array<Modifier> currentModifiers)
	{
		foreach (Modifier modifier in currentModifiers)
			modifier.GetDamage(this);
	}

	private async void OnDeath()
	{
		isGoingToDeath = true;
		StopThinking();
		await AnimateDeath();
		EmitSignal(SignalName.Death, this);

		QueueFree();
	}

	private void Animate(Vector3 targetPosition, float tweenDuration)
	{
		AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("Jump");

		Tween jumpTween = CreateTween();
		jumpTween.SetTrans(Tween.TransitionType.Cubic);
		jumpTween.SetEase(Tween.EaseType.InOut);
		jumpTween.TweenProperty(this, "position:y", targetPosition.y + jumpHeight, tweenDuration / 2);
		jumpTween.TweenProperty(this, "position:y", targetPosition.y, tweenDuration / 2);
		jumpTween.Parallel().TweenCallback(Callable.From(() => animationPlayer.PlayBackwards("Jump"))).SetDelay(tweenDuration / 2 - 0.3);
	}

	public void AnimateDamage()
	{
		if (!IsInstanceValid(this))
			return;

		AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("GetDamage");
	}

	private async Task AnimateDeath()
	{
		AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		await ToSignal(this, SignalName.FinishMoving);
		animationPlayer.Stop();
		animationPlayer.Play("Death");
		await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	}
}