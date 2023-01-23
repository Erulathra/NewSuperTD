using System;
using System.Collections.Generic;
using Godot;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Towers.Modifiers;

public partial class ElectricModifier : Modifier
{
	[Export()] public int SpreadDelayTickCount { get; private set; } = 2;

	public override void OnRegister(Tile tile, GlobalTickTimer globalTickTimer)
	{
		base.OnRegister(tile, globalTickTimer);
		
		void OnGlobalTick(int tickCount, GlobalTickTimer globalTickTimer)
		{
			if (tickCount % SpreadDelayTickCount == 0)
			{
				List<Tile> neighbors = tile.GetNeighbors();
				foreach (Tile neighbor in neighbors)
				{
					ModifierHandler modifierHandler = neighbor.GetNode<ModifierHandler>("ModifierHandler");
					if (!modifierHandler.Has("Water"))
						continue;
					
					if (modifierHandler.Has("Electric"))
						continue;
					
					modifierHandler.RegisterModifier("Electric");
				}
				globalTickTimer.GlobalTick -= OnGlobalTick;
			}
		}
		
		globalTickTimer.GlobalTick += OnGlobalTick;
	}
}