namespace NewSuperTD.Towers;

public partial class ElectricTower : Tower
{
	protected override void Think()
	{
		foreach (var tile in TilesInRange)
		{
			ModifierHandler modifierHandler = tile.GetNode<ModifierHandler>("ModifierHandler");
			modifierHandler.RegisterModifier("Electric");
			AnimateThinking();
		}
	}
}