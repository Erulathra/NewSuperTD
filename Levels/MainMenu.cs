using Godot;

namespace NewSuperTD.Levels;

public partial class MainMenu : Node
{
	public override void _Ready()
	{
		AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("Background/AnimationPlayer");
		animationPlayer.Play("Rotate");
	}

	void OnPressCreditsButton()
	{
		Control creditsPanel = GetNode<Control>("UI/CanvasLayer/CreditsWindow");
		creditsPanel.Visible = true;
	}

	void OnPressCreditsExitButton()
	{
		Control creditsPanel = GetNode<Control>("UI/CanvasLayer/CreditsWindow");
		creditsPanel.Visible = false;
	}

	void OnPressExitButton()
	{
		GetTree().Quit();
	}

	void OnPressPlayButton()
	{
		CanvasLayer levelSelector = GetNode<CanvasLayer>("UI/LevelSelector");
		levelSelector.Visible = true;
	}
}