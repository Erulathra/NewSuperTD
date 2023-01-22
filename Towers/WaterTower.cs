using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Towers;

public partial class WaterTower : Tower
{
	protected override void Think()
	{
		base.Think();

		foreach (Tile tile in TilesInRange)
		{
			ModifierHandler modifierHandler = tile.GetNode<ModifierHandler>("ModifierHandler");
			if (modifierHandler.RegisterModifier("Water") != null)
				AnimateThinking();
		}
	}
}