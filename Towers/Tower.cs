using Godot;
using Godot.Collections;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Towers;
public partial class Tower : Node3D
{
	[Export] public int Range { get; private set; } = 1;
	[Export] public int ThinkingTickCount { get; private set; } = 10;
	
	public Array<Tile> TilesInRange { get; private set; }
	public Tile OwnerTile { get; private set; }

	public override void _Ready()
	{
		Window root = GetTree().Root;
		GlobalTickTimer globalTickTimer = (GlobalTickTimer)root.FindChild("GlobalTickTimer", true, false);
		globalTickTimer.GlobalTick += OnGlobalTick;
		OwnerTile = GetNode<Tile>("..");
		
		TilesInRange = OwnerTile.GetTilesInRange(Range);
	}

	private void OnGlobalTick(int tickSum, GlobalTickTimer globalTickTimer)
	{
		if (tickSum % ThinkingTickCount == 0)
			Think();
	}

	protected virtual void Think()
	{ }

	public virtual void AnimateThinking()
	{
		AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("Attack");
	}
}