using Godot;

namespace NewSuperTD.Levels;

public partial class LevelSelectionMenu : CanvasLayer
{
	[Export] private PackedScene levelButtonPackedScene;
	
	private GridContainer grid;

	public override void _Ready()
	{
		grid = GetNode<GridContainer>("VBoxContainer/HBoxContainer/GridContainer");
		LevelManager levelManager = (LevelManager)GetTree().Root.FindChild("LevelManager", true, false);

		for (int i = 0; i < levelManager.GetLevelsCount(); i++)
			GenerateButton(i);
	}

	public void GenerateButton(int levelIndex)
	{
		LevelButton newBaseButton = (LevelButton)levelButtonPackedScene.Instantiate();
		newBaseButton.Name = $"Level{levelIndex}Button";
		newBaseButton.Text = $"{levelIndex + 1}";
		newBaseButton.LevelIndex = levelIndex;

		grid.AddChild(newBaseButton);
	}
	
	
}