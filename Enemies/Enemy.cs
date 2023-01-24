using System.Collections.Generic;
using System.Linq;
using Godot;
using NewSuperTD.Tiles.Scenes;
using NewSuperTD.Towers;
using NewSuperTD.Towers.Modifiers;

namespace NewSuperTD.Enemies;

public partial class Enemy : Node3D
{
	[Signal]
	public delegate void DeathEventHandler(Enemy enemy);

	[Signal]
	public delegate void ReachTargetEventHandler(Enemy enemy);

	private GlobalTickTimer globalTickTimer;
	[Export] private int moveTickCount = 10;

	private Tile targetTile;

	[Export] private int thinkingTickCount = 20;
	[Export] private float jumpHeight = 0.2f;

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

		List<Tile> neighbors = parentTile.GetNeighbors();
		targetTile = neighbors.OfType<PathTile>().First(pathNeighbor => pathNeighbor.DistanceToKing < distanceToKing);

		if (targetTile is KingTile)
		{
			EmitSignal(SignalName.ReachTarget, this);
		}

		StartMoving();
	}

	private void StartMoving()
	{
		PathTile parentTile = GetParent<PathTile>();
		Vector3 parentPosition = parentTile.GetNode<Node3D>("SurfaceHandle").GlobalPosition;
		Vector3 targetPosition = targetTile.GetNode<Node3D>("SurfaceHandle").GlobalPosition;

		float distanceBetweenParentAndTarget = (parentPosition - targetPosition).Length();
		float tweenDuration = ((float)globalTickTimer.WaitTime * moveTickCount);

		Callable moveCallable = Callable.From((Vector3 newPosition) => Move(newPosition, distanceBetweenParentAndTarget));
		Tween moveTween = CreateTween();
		moveTween.SetTrans(Tween.TransitionType.Sine);
		moveTween.SetEase(Tween.EaseType.InOut);
		moveTween.TweenMethod(moveCallable, parentPosition, targetPosition, tweenDuration);

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
		Reparent(targetTile);

		ModifierHandler targetModifierHandler = targetTile.GetNode<ModifierHandler>("ModifierHandler");
		foreach (Modifier modifier in targetModifierHandler.GetCurrentModifiersArray())
		{
			modifier.GetDamage(this);
		}
	}

	private async void Animate(Vector3 targetPosition, float tweenDuration)
	{
		Tween jumpTween = CreateTween();
		jumpTween.SetTrans(Tween.TransitionType.Cubic);
		jumpTween.SetEase(Tween.EaseType.InOut);
		jumpTween.TweenProperty(this, "position:y", targetPosition.y + jumpHeight, tweenDuration / 2);
		jumpTween.TweenProperty(this, "position:y", targetPosition.y, tweenDuration / 2);
		
		AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("Jump");
		await ToSignal(GetTree().CreateTimer(tweenDuration - 0.3), "timeout");
		animationPlayer.PlayBackwards("Jump");
	}

	public void AnimateDamage()
	{
		AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("GetDamage");
	}
}
