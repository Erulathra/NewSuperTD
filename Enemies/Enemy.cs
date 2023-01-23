using System.Collections.Generic;
using System.Linq;
using Godot;
using NewSuperTD.Tiles.Scenes;

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

	private void OnGlobalTick(int tickCount, GlobalTickTimer globalTickTimer)
	{
		if (tickCount % thinkingTickCount != 0)
			return;

		PathTile parentTile = GetParent<PathTile>();
		int distanceToKing = parentTile.DistanceToKing;

		List<Tile> neighbors = parentTile.GetNeighbors();
		targetTile = neighbors.OfType<PathTile>().First(pathNeighbor => pathNeighbor.DistanceToKing < distanceToKing);

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

	private void Animate(Vector3 targetPosition, float tweenDuration)
	{
		Tween jumpTween = CreateTween();
		jumpTween.SetTrans(Tween.TransitionType.Cubic);
		jumpTween.SetEase(Tween.EaseType.InOut);
		jumpTween.TweenProperty(this, "position:y", targetPosition.y + jumpHeight, tweenDuration / 2);
		jumpTween.TweenProperty(this, "position:y", targetPosition.y, tweenDuration / 2);
		
		AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("Jump");
	}

	private void Move(Vector3 position, float distanceBetweenParentAndTarget)
	{
		if (targetTile == null)
			return;

		GlobalPosition = position;

		float distanceToTarget = (GlobalPosition - targetTile.GlobalPosition).Length();
		if (distanceToTarget < 0.5 * distanceBetweenParentAndTarget)
			Reparent(targetTile);
	}
}