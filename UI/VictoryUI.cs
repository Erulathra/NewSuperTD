using Godot;
using NewSuperTD.Levels;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.UI;

public partial class VictoryUI : Control
{
	public override void _Ready()
	{
		TowerManager towerManager = GetParent().GetNode<TowerManager>("TowerManager");
		Level level = GetParent<Level>();

		if (towerManager.PlacedTowersCount <= level.TowersToThreeStars)
			SetStars(3);
		else if (towerManager.PlacedTowersCount <= level.TowersToTwoStars)
			SetStars(2);
		else
			SetStars(1);
	}

	public void SetStars(int count)
	{
		count = Mathf.Clamp(count, 0, 3);
		string filedStars = "";
		string emptyStars = "";

		for (int i = 0; i < count; i++)
		{
			filedStars += " ";
		}
		
		for (int i = 0; i < 3 - count; i++)
		{
			emptyStars += " ";
		}

		RichTextLabel starsLabel = GetNode<RichTextLabel>("CanvasLayer/Panel/Stars");
		starsLabel.Text = $"[color=yellow][wave][center][b]{filedStars}[/b][/center][/wave][/color][b]{emptyStars}[/b]";
	}

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
	
	public async void OnPressRepeatButton()
	{
		LevelManager levelManager = (LevelManager) GetTree().Root.FindChild("LevelManager", true, false);
		await levelManager.ReloadLevel();
	}
}