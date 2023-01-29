using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using NewSuperTD;
using NewSuperTD.Levels;

public partial class LevelManager : Node
{
	[Export(PropertyHint.ArrayType, "PackedScene")]
	private Array<PackedScene> Levels;
	[Export] private PackedScene mainMenuScene;
	
	public Level CurrentLevel { get; private set; }
	public int CurrentLevelIndex { get; private set; }
	
	private MainMenu mainMenu;

	public override void _Ready()
	{
		CurrentLevelIndex = -1;
		mainMenu = mainMenuScene.Instantiate<MainMenu>();
		AddChild(mainMenu);
	}

	public async void LoadLevel(int index)
	{
		await SceneTransitionOut();

		if (CurrentLevel != null)
		{
			CurrentLevel.QueueFree();
			CurrentLevel = null;
		}

		if (mainMenu != null)
		{
			mainMenu.QueueFree();
			mainMenu = null;
		}

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

		CurrentLevel = Levels[index].Instantiate<Level>();
		AddChild(CurrentLevel);
		CurrentLevelIndex = index;

		await SceneTransitionIn();
	}

	public async void LoadMainMenu()
	{
		await SceneTransitionOut();
		
		CurrentLevel.QueueFree();
		CurrentLevelIndex = -1;
		
		mainMenu = mainMenuScene.Instantiate<MainMenu>();
		AddChild(mainMenu);

		await SceneTransitionIn();
	}

	public async Task SceneTransitionIn()
	{
		GD.Print("IN");
	}
	
	public async Task SceneTransitionOut()
	{
		GD.Print("OUT");
	}
}