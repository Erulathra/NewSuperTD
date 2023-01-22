using System;
using Godot;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Towers.Modifiers;

public abstract partial class Modifier : Resource, ICloneable
{
	[Export] public Color ModifierColor { get; private set; }
	[Export] public int StayTickCount { get; private set; }
	[Export] public string ModifierId { get; private set; }
	protected int RegisterTickCount { get; private set; }
	private Tile ownerTile;

	protected Modifier() : this(Color.Color8(0, 255, 0, 125), 1, "NONE")
	{ }

	protected Modifier(Color modifierColor, int stayTickCount, string modifierId)
	{
		ModifierColor = modifierColor;
		StayTickCount = stayTickCount;
		RegisterTickCount = 0;
		ModifierId = modifierId;
	}
	public virtual void OnRegister(Tile tile, GlobalTickTimer globalTickTimer)
	{
		RegisterTickCount = globalTickTimer.TickCount;
		
		globalTickTimer.GlobalTick += OnGlobalTick;
		ownerTile = tile;
	}

	private void OnGlobalTick(int tickCount, GlobalTickTimer globalTickTimer)
	{
		if (tickCount > RegisterTickCount + StayTickCount)
		{
			ownerTile.GetNode<ModifierHandler>("ModifierHandler").UnregisterModifier(ModifierId);
			globalTickTimer.GlobalTick -= OnGlobalTick;
		}
	}

	public abstract void OnUnregister(Tile tile);

	public object Clone()
	{
		Modifier modifier = (Modifier)MemberwiseClone();
		return modifier;
	}
}