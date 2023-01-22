using Godot;
using System;
using NewSuperTD.Towers;

public partial class WaterTower : Tower
{
	protected override void Think()
	{
		base.Think();
		
		foreach (var tile in TilesInRange)
		{
			ModifierHandler modifierHandler = tile.GetNode<ModifierHandler>("ModifierHandler");
			if (modifierHandler.RegisterModifier("Water") != null)
				AnimateThinking();
		}
	}
}
