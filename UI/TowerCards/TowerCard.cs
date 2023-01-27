using Godot;
using System;

namespace NewSuperTD.UI.TowerCards;

[Tool]
public partial class TowerCard : Control
{
	[Signal] public delegate void ButtonUpEventHandler(string towerId, TowerCard towerCard);
	
	[Export] private string symbol = "#";
	[Export] private Color color = Colors.White;
	[Export] private double pickUpDuration = 0.1;
	[Export] private double pickUpDistance = -15;

	public string TowerId { get; set; } = "None";
	private Button button;
	private Tween moveTween;
	
	public override void _Ready()
	{
		button = GetNode<Button>("Button");
		button.Text = symbol;
		button.AddThemeColorOverride("font_pressed_color", color);
		button.AddThemeColorOverride("font_hover_color", color);
		button.AddThemeColorOverride("font_focus_color", color);

		button.ButtonUp += () => EmitSignal(SignalName.ButtonUp, TowerId, this);
	}
	
	
	public void Select()
	{
		moveTween?.Stop();
		moveTween = CreateTween();
		moveTween.SetTrans(Tween.TransitionType.Quad);
		moveTween.SetEase(Tween.EaseType.InOut);
		moveTween.TweenProperty(button, "position:y", pickUpDistance, pickUpDuration);
	}

	public void UnSelect()
	{
		moveTween?.Stop();
		moveTween = CreateTween();
		moveTween.SetTrans(Tween.TransitionType.Quad);
		moveTween.SetEase(Tween.EaseType.InOut);
		moveTween.TweenProperty(button, "position:y", 0, pickUpDuration);
	}

	public override void _Process(double delta)
	{
	}
}
