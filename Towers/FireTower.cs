using System;
using Godot;
using NewSuperTD.Tiles.Scenes;
using NewSuperTD.Towers;

public partial class FireTower : Tower
{
	protected override void Think()
	{
		foreach (var tile in TilesInRange)
		{
			ModifierHandler modifierHandler = tile.GetNode<ModifierHandler>("ModifiersHandler");
			modifierHandler.RegisterModifier("Fire");
		}
	}
}