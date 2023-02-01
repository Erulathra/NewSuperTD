using Godot;

namespace NewSuperTD.Levels;

public partial class LevelButton : Button
{
	public int LevelIndex { get; set; } = -1;
	public async void OnPress()
	{
		LevelManager levelManager = (LevelManager)GetTree().Root.FindChild("LevelManager", true, false);
		await levelManager.LoadLevel(LevelIndex);
	}
}
