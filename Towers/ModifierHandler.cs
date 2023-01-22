using Godot;
using Godot.Collections;
using NewSuperTD.Tiles.Scenes;
using NewSuperTD.Towers.Modifiers;
using Array = System.Array;

namespace NewSuperTD.Towers;

public partial class ModifierHandler : Node
{
	[Signal]
	public delegate void OnRegisterModifierEventHandler(Array<Modifier> currentModifiers);

	private Godot.Collections.Dictionary<string, Modifier> actualModifiers;
	private GlobalTickTimer globalTickTimer;

	private ModifiersManager modifiersManager;
	private Tile parentTile;

	public override void _Ready()
	{
		base._Ready();
		modifiersManager = (ModifiersManager)GetTree().Root.FindChild("ModifiersManager", true, false);
		globalTickTimer = (GlobalTickTimer)GetTree().Root.FindChild("GlobalTickTimer", true, false);

		actualModifiers = new Godot.Collections.Dictionary<string, Modifier>();

		parentTile = GetNode<Tile>("..");
	}

	public bool Has(string modifierId)
	{
		return actualModifiers.ContainsKey(modifierId);
	}

	public Modifier RegisterModifier(string modifierId)
	{
		if (!modifiersManager.ModifiersDictionary.ContainsKey(modifierId))
		{
			GD.PrintErr($"Modifier {modifierId}, doesnt exist.");
			return null;
		}
		
		if (actualModifiers.ContainsKey(modifierId))
			return null;

		Modifier modifier = (Modifier)modifiersManager.ModifiersDictionary[modifierId].Clone();
		actualModifiers.Add(modifierId, modifier);
		modifier.OnRegister(parentTile, globalTickTimer);

		Array<Modifier> currentModifiersArray = GetCurrentModifiersArray();
		EmitSignal(SignalName.OnRegisterModifier, currentModifiersArray);
		parentTile.ModifiersColor = GetModiferColor(currentModifiersArray);

		return modifier;
	}
	

	public void UnregisterModifier(string modifierId)
	{
		if (!modifiersManager.ModifiersDictionary.ContainsKey(modifierId))
		{
			GD.PrintErr($"Modifier {modifierId}, doesnt exist.");
			return;
		}
		
		if (!actualModifiers.ContainsKey(modifierId))
			return;

		Modifier modifier = actualModifiers[modifierId];
		modifier.OnUnregister(parentTile, globalTickTimer);
		actualModifiers.Remove(modifierId);
		
		parentTile.ModifiersColor = GetModiferColor(GetCurrentModifiersArray());
	}

	public Array<Modifier> GetCurrentModifiersArray()
	{
		Array<Modifier> result = new Array<Modifier>();
		foreach (Modifier modifier in actualModifiers.Values)
		{
			result.Add(modifier);
		}

		return result;
	}

	public Color GetModiferColor(Array<Modifier> modifiers)
	{
		Color result = new Color(0);
		foreach (Modifier modifier in modifiers)
		{
			result = result.Blend(modifier.ModifierColor);
		}

		return result;
	}
}