using Godot;
using System;

public partial class Level : Node3D
{
	[Export] public int TowersToThreeStars { get; private set; }
	[Export] public int TowersToTwoStars { get; private set; }
}
