using Godot;

namespace NewSuperTD.UI;

public partial class VictoryUI : Control
{
	public async void OnPressMenuButton()
	{
		LevelManager levelManager = (LevelManager) GetTree().Root.FindChild("LevelManager", true, false);
		await levelManager.LoadMainMenu();
	}
	
	public async void OnPressContinueButton()
	{
		LevelManager levelManager = (LevelManager) GetTree().Root.FindChild("LevelManager", true, false);
		int currentLevelIndex = levelManager.CurrentLevelIndex;
		await levelManager.LoadLevel(currentLevelIndex + 1);
	}
}