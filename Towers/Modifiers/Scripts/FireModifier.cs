using Godot;
using NewSuperTD.Enemies;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Towers.Modifiers;

public partial class FireModifier : Modifier
{
	[Export] public int Damage = 20;

	private Tile ownerTile;

	public override void OnRegister(Tile tile, GlobalTickTimer globalTickTimer)
	{
		base.OnRegister(tile, globalTickTimer);
		ModifierHandler modifierHandler = tile.GetNode<ModifierHandler>("ModifierHandler");

		if (modifierHandler.Has("Water"))
		{
			modifierHandler.UnregisterModifier(ModifierId);
			modifierHandler.UnregisterModifier("Water");
		}
	}

	public override void GetDamage(Enemy enemy)
	{
		enemy.AnimateDamage();
		enemy.HealthPoints -= Damage;
	}
}