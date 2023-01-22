using Godot;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Towers.Modifiers;

public partial class FireModifier : Modifier
{
	[Export] public int Damage;

	private Tile ownerTile;

	public override void OnRegister(Tile tile, GlobalTickTimer globalTickTimer)
	{
		base.OnRegister(tile, globalTickTimer);
	}

	public override void OnUnregister(Tile tile)
	{ }
}