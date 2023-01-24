using System;
using System.Linq;
using Godot;
using Godot.Collections;
using NewSuperTD.Enemies;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Managers;

public partial class EnemyManager : Node
{
	[Signal] public delegate void EnemyReachTargetEventHandler();
	
	[Export] private int spawnTickCount = 20;
	[Export] private Dictionary<PackedScene, int> enemyDictionary = new();
	
	private PathTile startTile;
	private KingTile kingTile;
	public override void _Ready()
	{
		Array<Node> tiles = GetParent<Node>().GetNode("Tiles").GetChildren();
		startTile = tiles.OfType<PathTile>().MaxBy(pathTile => pathTile.DistanceToKing);
		kingTile = tiles.OfType<KingTile>().First();
		
		GlobalTickTimer globalTickTimer = (GlobalTickTimer)GetTree().Root.FindChild("GlobalTickTimer", true, false);
		globalTickTimer.GlobalTick += OnGlobalTick;
	}

	private void OnGlobalTick(int tickCount, GlobalTickTimer globalTickTimer)
	{
		if ((tickCount + 7) % spawnTickCount != 0)
			return;
		
		PackedScene[] enemiesTypes = enemyDictionary.Keys.ToArray();
		
		if (enemiesTypes.Length <= 0)
			return;

		PackedScene enemyToSpawnScene = enemiesTypes[GD.Randi() % enemiesTypes.Length];
		enemyDictionary[enemyToSpawnScene]--;

		if (enemyDictionary[enemyToSpawnScene] == 0)
			enemyDictionary.Remove(enemyToSpawnScene);

		Enemy newEnemy = (Enemy)enemyToSpawnScene.Instantiate();
		startTile.AddChild(newEnemy);
		newEnemy.GlobalPosition = startTile.GetNode<Node3D>("SurfaceHandle").GlobalPosition;

		newEnemy.ReachTarget += OnEnemyReachTarget;
		EnemyReachTarget += newEnemy.StopThinking;
	}

	private void OnEnemyReachTarget(Enemy enemy)
	{
		GlobalTickTimer globalTickTimer = (GlobalTickTimer)GetTree().Root.FindChild("GlobalTickTimer", true, false);
		globalTickTimer.GlobalTick -= OnGlobalTick;
		
		kingTile.GameOver();
		EmitSignal(SignalName.EnemyReachTarget);
	}
}