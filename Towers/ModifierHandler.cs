using System;
using System.Collections.Generic;
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
	
	private Godot.Collections.Dictionary<String, Modifier> modifiersDictionary;
	private Tile parentTile;

	private HashSet<String> actualModifiers;

	public override void _Ready()
	{
		base._Ready();
		actualModifiers = new HashSet<string>();
		
		parentTile = GetNode<Tile>("..");
	}

	public bool Has(String modifierId)
	{
		return actualModifiers.Contains(modifierId);
	}

	public void RegisterModifier(String modifierId)
	{
		if (!modifiersDictionary.ContainsKey(modifierId))
		{
			GD.PrintErr($"Modifier {modifierId}, doesnt exist.");
			return;
		}

		actualModifiers.Add(modifierId);

		EmitSignal(SignalName.OnRegisterModifier, GetCurrentModifiersArray());
	}

	public void UnregisterModifier(String modifierId)
	{
		if (!modifiersDictionary.ContainsKey(modifierId))
		{
			GD.PrintErr($"Modifier {modifierId}, doesnt exist.");
			return;
		}
		
		actualModifiers.Remove(modifierId);
	}

	public Array<Modifier> GetCurrentModifiersArray()
	{
		Array<Modifier> currentModifiers = new();
		foreach (var modifier in actualModifiers)
		{
			currentModifiers.Add(modifiersDictionary[modifier]);
		}

		return currentModifiers;
	}
}