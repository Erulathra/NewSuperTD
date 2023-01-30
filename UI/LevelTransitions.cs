using Godot;
using System.Threading.Tasks;

public partial class LevelTransitions : Control
{
	private AnimationPlayer animationPlayer;
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public async Task AnimateIn()
	{
		animationPlayer.Play("WalkIn");
		await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	}
	
	public async Task AnimateOut()
	{
		animationPlayer.Play("WalkOut");
		await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
	}
}
