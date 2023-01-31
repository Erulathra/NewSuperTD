using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace NewSuperTD.Levels;

public partial class ChooseLevelMenu : Node
{
	[Export] int totalLevels = 0;
	[Export] PackedScene baseButton;
	private GridContainer grid;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		grid = GetNode<GridContainer>("VBoxContainer/HBoxContainer/GridContainer");

		if (!(totalLevels <= 4))
		{
			ColumnSize();
		}
		
		for (int i = 0; i < totalLevels; i++)
		{
			GenerateButtons(i + 1);
		}
	}

	public void GenerateButtons(int name)
	{
		Button bb = (Button)baseButton.Instantiate();
		bb.Name = name.ToString();
		bb.Text = name.ToString();
		bb.SceneFilePath = ("res://Levels/" + name + ".tscn");
		grid.AddChild(bb);
	}

	public void ColumnSize()
	{
		if (totalLevels % 2 == 0)
		{
			grid.Columns = totalLevels / 2;
		}
		else
		{
			grid.Columns = totalLevels / 2 + 1;
		}
	}
	
}
