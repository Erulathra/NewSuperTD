using System;
using System.Collections.Generic;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Towers.Modifiers;

public partial class ElectricModifier : Modifier
{
	public override void OnRegister(Tile tile, GlobalTickTimer globalTickTimer)
	{
		base.OnRegister(tile, globalTickTimer);

		List<Tile> neighbors = tile.GetNeighbours();
		foreach (Tile neighbor in neighbors)
		{
			ModifierHandler modifierHandler = neighbor.GetNode<ModifierHandler>("ModifierHandler");
			if (modifierHandler.Has("Water") && !modifierHandler.Has("Electric"))
			{
				modifierHandler.RegisterModifier("Electric");
			}
		}
	}

	public override void OnUnregister(Tile tile)
	{
	}
}