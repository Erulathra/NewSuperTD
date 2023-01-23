using System.Collections.Generic;
using Godot;
using Godot.Collections;
using NewSuperTD.Managers;
using NewSuperTD.Tiles.Scenes;
using NewSuperTD.Towers.Modifiers;
using Array = System.Array;

namespace NewSuperTD.Towers;

public partial class ModifierHandler : Node
{
	[Signal]
	public delegate void OnRegisterModifierEventHandler(Array<Modifier> currentModifiers);

	private HashSet<string> actualModifiers;
	private GlobalTickTimer globalTickTimer;

	private ModifiersManager modifiersManager;
	private Tile parentTile;

	public override void _Ready()
	{
		base._Ready();
		modifiersManager = (ModifiersManager)GetTree().Root.FindChild("ModifiersManager", true, false);
		globalTickTimer = (GlobalTickTimer)GetTree().Root.FindChild("GlobalTickTimer", true, false);

		actualModifiers = new();

		parentTile = GetParent<Tile>();
	}

	public bool Has(string modifierId)
	{
		return actualModifiers.Contains(modifierId);
	}

	public bool RegisterModifier(string modifierId)
	{
		if (!modifiersManager.ModifiersDictionary.ContainsKey(modifierId))
		{
			GD.PrintErr($"Modifier {modifierId}, doesnt exist.");
			return false;
		}
		
		if (actualModifiers.Contains(modifierId))
			return false;

		Modifier modifier = modifiersManager.ModifiersDictionary[modifierId];
		actualModifiers.Add(modifierId);
		modifier.OnRegister(parentTile, globalTickTimer);

		Array<Modifier> currentModifiersArray = GetCurrentModifiersArray();
		EmitSignal(SignalName.OnRegisterModifier, currentModifiersArray);
		parentTile.ModifiersColor = GetModiferColor(currentModifiersArray);

		return true;
	}
	

	public void UnregisterModifier(string modifierId)
	{
		if (!modifiersManager.ModifiersDictionary.ContainsKey(modifierId))
		{
			GD.PrintErr($"Modifier {modifierId}, doesnt exist.");
			return;
		}
		
		if (!actualModifiers.Contains(modifierId))
			return;

		modifiersManager.ModifiersDictionary[modifierId].OnUnregister(parentTile, globalTickTimer);
		actualModifiers.Remove(modifierId);
		
		parentTile.ModifiersColor = GetModiferColor(GetCurrentModifiersArray());
	}

	public Array<Modifier> GetCurrentModifiersArray()
	{
		Array<Modifier> result = new();
		foreach (string modifierId in actualModifiers)
		{
			result.Add(modifiersManager.ModifiersDictionary[modifierId]);
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