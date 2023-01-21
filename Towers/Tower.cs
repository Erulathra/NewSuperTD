using Godot;
using System;

public partial class Tower : Node3D
{
	[Export()]
	public int Range { get; private set; } = 1;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
