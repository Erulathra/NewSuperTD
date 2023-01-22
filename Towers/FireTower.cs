using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Towers;

public partial class FireTower : Tower
{
	protected override void Think()
	{
		foreach (Tile tile in TilesInRange)
		{
			ModifierHandler modifierHandler = tile.GetNode<ModifierHandler>("ModifierHandler");
			modifierHandler.RegisterModifier("Fire");
			AnimateThinking();
		}
	}
}