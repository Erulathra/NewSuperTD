using Godot;

namespace NewSuperTD.Managers;

public partial class EnemyLevelScriptEntry : Resource
{
	[Export()] public PackedScene EnemyPackedScene { get; set; }
	[Export()] public int Amount { get; set; }
}