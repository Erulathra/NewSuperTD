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

	[ExportGroup("Logic")]
	[Export] private int healthPoints = 100;
	[Export] private int thinkingTickCount = 20;

	[ExportGroup("Animation")]
	[Export] private float jumpHeight = 0.2f;
	[Export] private int moveTickCount = 10;
	[Export] private int damageToGreatDamageAnimation = 20;

	private bool isGoingToDeath;
	private Tween moveTween;
	private Tile targetTile;
	
	private GlobalTickTimer globalTickTimer;

	public int HealthPoints
	{
		get => healthPoints;
		set
		{
			if (healthPoints - value >= damageToGreatDamageAnimation)
				AnimateGreatDamage();
			else if (healthPoints - value > 0)
				AnimateDamage();
			
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
		
		Tile newTargetTile = FindNextTile(tickCount);

		if (newTargetTile is KingTile)
			EmitSignal(SignalName.ReachTarget, this);
		
		if (!CanMove(newTargetTile, tickCount))
			return;

		targetTile = newTargetTile;
		StartMoving();
	}

	protected virtual Tile FindNextTile(int tickCount)
	{
		PathTile parentTile = GetParent<PathTile>();
		int distanceToKing = parentTile.DistanceToKing;
		Array<Tile> neighbors = parentTile.GetNeighbors();
		return neighbors.OfType<PathTile>().First(pathNeighbor => pathNeighbor.DistanceToKing < distanceToKing);
	}

	public bool CanMove(Tile newTargetTile, int tickCount)
	{
		if (tickCount % thinkingTickCount != 0)
			return false;

		if (newTargetTile == null)
			return false;

		Enemy targetTileEnemy = newTargetTile.GetEnemy();
		if (targetTileEnemy == null)
			return true;

		if (targetTileEnemy == this)
			return false;
		
		return targetTileEnemy.CanMove(newTargetTile, tickCount);
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
	
	private void Animate(Vector3 targetPosition, float tweenDuration)
	{
		AnimationTree animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Set("parameters/AirTransition/transition_request", "Jumping");
		animationTree.Set("parameters/JumpingTransition/transition_request", "Jumping");

		Tween jumpTween = CreateTween();
		jumpTween.SetTrans(Tween.TransitionType.Cubic);
		jumpTween.SetEase(Tween.EaseType.InOut);
		jumpTween.TweenProperty(this, "position:y", targetPosition.Y + jumpHeight, tweenDuration / 2);
		jumpTween.TweenProperty(this, "position:y", targetPosition.Y, tweenDuration / 2);
		jumpTween.Parallel().TweenCallback(Callable.From(() =>
		{
			animationTree.Set("parameters/AirTransition/transition_request", "Landing");
			animationTree.Set("parameters/JumpingTransition/transition_request", "Jumping");
		})).SetDelay(tweenDuration / 2 - 0.3);
	}

	private void AnimateDamage()
	{
		if (!IsInstanceValid(this))
			return;

		AnimationTree animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Set("parameters/DamageType/transition_request", "Damage");
		animationTree.Set("parameters/DamageOneShot/request", 1);
	}

	private void AnimateGreatDamage()
	{
		if (!IsInstanceValid(this))
			return;
		
		AnimationTree animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Set("parameters/DamageType/transition_request", "GreatDamage");
		animationTree.Set("parameters/DamageOneShot/request", 1);
	}

	private async void OnDeath()
	{
		isGoingToDeath = true;
		StopThinking();
		await AnimateDeath();

		AnimationTree animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.AnimationFinished += animationName =>
		{
			if (animationName != "Death")
				return;

			EmitSignal(SignalName.Death, this);

			QueueFree();
		};
	}

	private async Task AnimateDeath()
	{
		AnimationTree animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Set("parameters/LivingTransition/transition_request", "Death");
	}

	public override void _ExitTree()
	{
		if (IsQueuedForDeletion())
		{
			MeshInstance3D mesh = GetNode<MeshInstance3D>("Pawn");
			mesh.SetSurfaceOverrideMaterial(0, null);
		}
	}
}