using Godot;
using Godot.Collections;
using NewSuperTD;
using NewSuperTD.Levels;

public partial class LevelManager : Node
{
	[Export(PropertyHint.ArrayType, "PackedScene")]
	private Array<PackedScene> Levels;
	[Export] private PackedScene mainMenuScene;
	
	private MainMenu mainMenu;
	private Level currentLevel;
	private int currentLevelIndex;

	public override void _Ready()
	{
		mainMenu = mainMenuScene.Instantiate<MainMenu>();
		AddChild(mainMenu);
	}

	public void LoadLevel(int index)
	{
		GlobalTickTimer globalTickTimer = (GlobalTickTimer)GetTree().Root.FindChild("GlobalTickTimer", true, false);
		globalTickTimer.StopAndResetTickCount();
		GD.Print($"Load {index} level");
	}
}