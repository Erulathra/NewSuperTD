using System.Linq;
using Godot;
using Godot.Collections;
using NewSuperTD.Enemies;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Managers;

public partial class EnemyManager : Node
{
	[Signal] public delegate void EnemyReachTargetEventHandler();

	[Signal] public delegate void GameOverEventHandler();
	[Signal] public delegate void AllEnemiesAreDeadEventHandler();

	[Export] private Dictionary<PackedScene, int> enemyDictionary;
	[Export] private int spawnTickCount = 20;
	
	private KingTile kingTile;
	private Array<Enemy> enemiesAlive = new();
	// Hack to bypass exported Dictionary godot bug
	private Dictionary<PackedScene, int> currentEnemyDictionary;

	private PathTile startTile;

	public override void _Ready()
	{
		Array<Node> tiles = GetParent<Node>().GetNode("Tiles").GetChildren();
		startTile = tiles.OfType<PathTile>().MaxBy(pathTile => pathTile.DistanceToKing);
		kingTile = tiles.OfType<KingTile>().First();

		currentEnemyDictionary = new(enemyDictionary);

		GlobalTickTimer globalTickTimer = (GlobalTickTimer)GetTree().Root.FindChild("GlobalTickTimer", true, false);
		globalTickTimer.GlobalTick += OnGlobalTick;
	}

	private void OnGlobalTick(int tickCount, GlobalTickTimer globalTickTimer)
	{
		if ((tickCount + 7) % spawnTickCount != 0)
			return;

		PackedScene[] enemiesTypes = currentEnemyDictionary.Keys.ToArray();

		if (enemiesTypes.Length <= 0)
			return;

		PackedScene enemyToSpawnScene = enemiesTypes[GD.Randi() % enemiesTypes.Length];
		currentEnemyDictionary[enemyToSpawnScene]--;

		if (currentEnemyDictionary[enemyToSpawnScene] == 0)
			currentEnemyDictionary.Remove(enemyToSpawnScene);

		Enemy newEnemy = (Enemy)enemyToSpawnScene.Instantiate();
		startTile.AddChild(newEnemy);
		newEnemy.GlobalPosition = startTile.GetNode<Node3D>("SurfaceHandle").GlobalPosition;

		enemiesAlive.Add(newEnemy);

		newEnemy.ReachTarget += OnEnemyReachTarget;
		newEnemy.Death += OnEnemyDeath;
		EnemyReachTarget += newEnemy.StopThinking;
	}

	private async void OnEnemyReachTarget(Enemy enemy)
	{
		GlobalTickTimer globalTickTimer = (GlobalTickTimer)GetTree().Root.FindChild("GlobalTickTimer", true, false);
		globalTickTimer.GlobalTick -= OnGlobalTick;

		kingTile.GameOver();
		EmitSignal(SignalName.EnemyReachTarget);
		await ToSignal(kingTile.GetNode<AnimationPlayer>("King/AnimationPlayer"), AnimationPlayer.SignalName.AnimationFinished);
		EmitSignal(SignalName.GameOver);
	}

	private void OnEnemyDeath(Enemy enemy)
	{
		enemiesAlive.Remove(enemy);
		
		if (enemiesAlive.Count <= 0)
			EmitSignal(SignalName.AllEnemiesAreDead);
	}
}