using System;
using System.Linq;
using Godot;
using Godot.Collections;
using NewSuperTD.Enemies;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Managers;

public partial class EnemyManager : Node
{
	[Export] private int spawnTickCount = 20;
	[Export] private Dictionary<PackedScene, int> enemyDictionary = new();
	
	private PathTile startTile;
	public override void _Ready()
	{
		Array<Node> tiles = GetParent<Node>().GetNode("Tiles").GetChildren();
		startTile = tiles.OfType<PathTile>().MaxBy(pathTile => pathTile.DistanceToKing);
		GlobalTickTimer globalTickTimer = (GlobalTickTimer)GetTree().Root.FindChild("GlobalTickTimer", true, false);
		globalTickTimer.GlobalTick += OnGlobalTick;
	}

	private void OnGlobalTick(int tickCount, GlobalTickTimer globalTickTimer)
	{
		if ((tickCount + 7) % spawnTickCount != 0)
			return;

		PackedScene[] enemiesTypes = enemyDictionary.Keys.ToArray();
		PackedScene enemyToSpawnScene = enemiesTypes[GD.Randi() % enemiesTypes.Length];
		enemyDictionary[enemyToSpawnScene]--;

		if (enemyDictionary[enemyToSpawnScene] == 0)
			enemyDictionary.Remove(enemyToSpawnScene);

		Enemy newEnemy = (Enemy)enemyToSpawnScene.Instantiate();
		startTile.AddChild(newEnemy);
		newEnemy.GlobalPosition = startTile.GetNode<Node3D>("SurfaceHandle").GlobalPosition;
	}
}