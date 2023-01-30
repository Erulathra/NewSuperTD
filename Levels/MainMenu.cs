using Godot;

namespace NewSuperTD.Levels;

public partial class MainMenu : Node
{
	public override void _Ready()
	{
		AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("Node3D/AnimationPlayer");
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

	async void OnPressPlayButton()
	{
		await GetParent<LevelManager>().LoadLevel(2);
	}
}