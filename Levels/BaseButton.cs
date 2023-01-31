using System;
using Godot;

namespace NewSuperTD.Levels;

public partial class BaseButton : Button
{
	/*private Button button;
	public override void _Ready()
	{
		button = GetNode<Button>(".");
		
	}*/
	public void ChangeLevelPress()
	{
		 //GetTree().ChangeSceneToFile(button.SceneFilePath);
		GetTree().ChangeSceneToFile("res://MainScene.tscn");
	}
}
