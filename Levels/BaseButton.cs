using System;
using Godot;

namespace NewSuperTD.Levels;

public partial class BaseButton : Control
{
	
	public async void ChangeLevelPress()
	{
		LevelManager levelManager = (LevelManager) GetTree().Root.FindChild("LevelManager", true, false);
		await levelManager.LoadLevel(2);
	}
}
