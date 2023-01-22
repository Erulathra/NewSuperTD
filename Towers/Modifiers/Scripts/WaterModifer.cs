using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Towers.Modifiers;

public partial class WaterModifer : Modifier
{
	public override void OnRegister(Tile tile, GlobalTickTimer globalTickTimer)
	{
		base.OnRegister(tile, globalTickTimer);
		ModifierHandler modifierHandler = tile.GetNode<ModifierHandler>("ModifierHandler");

		if (modifierHandler.Has("Fire"))
		{
			modifierHandler.UnregisterModifier(ModifierId);
			modifierHandler.UnregisterModifier("Fire");
		}
	}

	public override void OnUnregister(Tile tile)
	{
	}
}