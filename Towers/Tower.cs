using System.Collections.Generic;
using Godot;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Towers;
public partial class Tower : Node3D
{
	[Export()]
	public int Range { get; private set; } = 1;

	[Export()] public int ThinkingTickCount { get; private set; } = 10;
	
	public List<Tile> TilesInRange { get; private set; }
	public Tile OwnerTile { get; private set; }

	public override void _Ready()
	{
		Window root = GetTree().Root;
		GlobalTickTimer globalTickTimer = (GlobalTickTimer)root.FindChild("GlobalTickTimer", true, false);
		globalTickTimer.GlobalTick += OnGlobalTick;
		OwnerTile = GetNode<Tile>("..");
		
		TilesInRange = OwnerTile.GetTilesInRange(Range);
		
		Think();
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
		
	}
}