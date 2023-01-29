using Godot;
using Godot.Collections;
using NewSuperTD.Levels;

public partial class LevelManager : Node
{
	[Export(PropertyHint.ArrayType, "PackedScene" )] private Array<PackedScene> Levels;
	[Export] private PackedScene mainMenuScene;
	
	private Level currentLevel;
	private MainMenu mainMenu;

	public override void _Ready()
	{
		mainMenu = mainMenuScene.Instantiate<MainMenu>();
		AddChild(mainMenu);
	}

}