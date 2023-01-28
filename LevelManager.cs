using Godot;
using Godot.Collections;
using NewSuperTD.Levels;

public partial class LevelManager : Node
{
	[Export(PropertyHint.ArrayType, "PackedScene" )] private Array<PackedScene> Levels;
	[Export] private PackedScene mainMenuScene;
	
	private Level currentLevel;
	
	public override void _Ready()
	{ }

}