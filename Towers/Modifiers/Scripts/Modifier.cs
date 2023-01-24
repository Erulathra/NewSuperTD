using Godot;
using NewSuperTD.Enemies;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Towers.Modifiers;

public abstract partial class Modifier : Resource
{
	[Export] public Color ModifierColor { get; private set; }
	[Export] public int StayTickCount { get; private set; }
	[Export] public string ModifierId { get; private set; }
	protected int RegisterTickCount { get; private set; }

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
		AnimateRegister(tile);

		void OnGlobalTick(int tickCount, GlobalTickTimer _)
		{
			if (tickCount <= RegisterTickCount + StayTickCount) return;

			tile.GetNode<ModifierHandler>("ModifierHandler").UnregisterModifier(ModifierId);
			globalTickTimer.GlobalTick -= OnGlobalTick;
		}

		globalTickTimer.GlobalTick -= OnGlobalTick;
		globalTickTimer.GlobalTick += OnGlobalTick;
	}

	public virtual void OnUnregister(Tile tile, GlobalTickTimer globalTickTimer)
	{
	}

	public virtual void AnimateRegister(Tile tile)
	{
		AnimationPlayer animationPlayer = tile.GetNode<AnimationPlayer>("AnimationPlayer");
		
		if (animationPlayer.IsPlaying())
			animationPlayer.Seek(0 );
		
		animationPlayer.Play("RegisterModifier");
	}

	public virtual void GetDamage(Enemy enemy)
	{
	}
}