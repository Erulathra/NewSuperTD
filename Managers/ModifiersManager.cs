using Godot;
using System;
using Godot.Collections;
using NewSuperTD.Towers.Modifiers;

public partial class ModifiersManager : Node
{
	[Export()] public Dictionary<String, Modifier> ModifiersDictionary { get; private set; } = new Dictionary<string, Modifier>();
	
}
