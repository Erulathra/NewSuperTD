using Godot;

namespace NewSuperTD.Levels;

public partial class MainMenu : Node
{
	public override void _Ready()
	{
		AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("Node3D/AnimationPlayer");
		animationPlayer.Play("Rotate");
	}
}