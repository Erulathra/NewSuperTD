using Godot;
using Godot.Collections;
using NewSuperTD.Enemies;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Towers.Modifiers;

public partial class ElectricModifier : Modifier
{
	[Export()] public int SpreadDelayTickCount { get; private set; } = 1;
	[Export()] private int damage = 10;

	public override void OnRegister(Tile tile, GlobalTickTimer globalTickTimer)
	{
		base.OnRegister(tile, globalTickTimer);
		
		void OnGlobalTick(int tickCount, GlobalTickTimer globalTickTimer)
		{
			if (tickCount % SpreadDelayTickCount == 0)
			{
				Array<Tile> neighbors = tile.GetNeighbors();
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

	public override void GetDamage(Enemy enemy)
	{
		int finalDamage = damage;

		if (enemy is IElectricResistance)
			finalDamage /= 2;
		
		enemy.HealthPoints -= finalDamage;
		enemy.AnimateDamage();
	}
}