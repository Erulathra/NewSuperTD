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

	public int GetLevelsCount()
	{
		return Levels.Count;
	}

	public async Task ReloadLevel()
	{
		await LoadLevel(CurrentLevelIndex);
	}
	
	public async Task LoadLevel(int index)
	{
		await SceneTransitionIn();

		CurrentLevel?.QueueFree();

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

		index = Mathf.Wrap(index, 0, Levels.Count);

		PackedScene packedLevel = GD.Load<PackedScene>(Levels[index]);
		Node newLevel = packedLevel.Instantiate();
		AddChild(newLevel);
		CurrentLevelIndex = index;

		CurrentLevel = newLevel;

		await SceneTransitionOut();
	}

	public async Task LoadMainMenu()
	{
		await SceneTransitionIn();
		
		CurrentLevel?.QueueFree();
		CurrentLevelIndex = -1;

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		
		CurrentLevel = mainMenuScene.Instantiate();
		AddChild(CurrentLevel);

		await SceneTransitionOut();
	}

	public async Task SceneTransitionIn()
	{
		LevelTransitions levelTransitions = (LevelTransitions) GetTree().Root.FindChild("SceneTransitions", true, false);
		await levelTransitions.AnimateIn();
	}
	
	public async Task SceneTransitionOut()
	{
		LevelTransitions levelTransitions = (LevelTransitions) GetTree().Root.FindChild("SceneTransitions", true, false);
		await levelTransitions.AnimateOut();
	}
}