using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using NewSuperTD.Levels;

public partial class LevelManager : Node
{
	[Export(PropertyHint.File, "*.tscn")]
	private Array<string> Levels;
	[Export] private PackedScene mainMenuScene;
	
	public Node CurrentLevel { get; private set; }
	public int CurrentLevelIndex { get; private set; }
	
	public override void _Ready()
	{
		CurrentLevelIndex = -1;
		CurrentLevel = mainMenuScene.Instantiate<MainMenu>();
		AddChild(CurrentLevel);
	}

	public async Task ReloadLevel()
	{
		await LoadLevel(CurrentLevelIndex);
	}

	public async Task LoadLevel(int index)
	{
		await SceneTransitionOut();

		CurrentLevel?.QueueFree();

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

		PackedScene packedLevel = GD.Load<PackedScene>(Levels[index]);
		var newLevel = packedLevel.Instantiate();
		AddChild(newLevel);
		CurrentLevelIndex = index;

		CurrentLevel = newLevel;

		await SceneTransitionIn();
	}

	public async Task LoadMainMenu()
	{
		CurrentLevel?.QueueFree();
		CurrentLevelIndex = -1;

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		
		CurrentLevel = mainMenuScene.Instantiate();
		AddChild(CurrentLevel);

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