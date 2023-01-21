using Godot;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Towers.Modifiers;

public abstract class Modifier : Object
{
	public Color ModifierColor {get; private set; } = Color.Color8(0, 255, 0, 125);
	
	public abstract void OnRegister(Tile tile);
	public abstract void OnUnregister(Tile tile);
}