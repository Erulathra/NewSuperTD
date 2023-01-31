using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace NewSuperTD.Levels;

public partial class ChooseLevelMenu : Node
{
	[Export] int totalLevels;
	[Export] PackedScene baseButton;
	private GridContainer grid;
	
	public override void _Ready()
	{
		grid = GetNode<GridContainer>("VBoxContainer/HBoxContainer/GridContainer");

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
		bb.SceneFilePath = ("res://Levels/" + name + ".tscn");	//tu jest jakby levele miały nazwę 1.tscn, 2.tscn itd.
		grid.AddChild(bb);
	}

}
