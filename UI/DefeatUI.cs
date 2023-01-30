using Godot;

namespace NewSuperTD.UI;

public partial class DefeatUI : Control
{
	public async void OnPressMenuButton()
	{
		LevelManager levelManager = (LevelManager) GetTree().Root.FindChild("LevelManager", true, false);
		await levelManager.LoadMainMenu();
	}
	
	public async void OnPressReloadButton()
	{
		LevelManager levelManager = (LevelManager) GetTree().Root.FindChild("LevelManager", true, false);
		await levelManager.ReloadLevel();
	}
}