using Godot;
using NewSuperTD.Managers;

namespace NewSuperTD.Levels;

public partial class Level : Node3D
{
	[Export] public int TowersToThreeStars { get; private set; }
	[Export] public int TowersToTwoStars { get; private set; }

	[Export] private PackedScene victoryScene;
	[Export] private PackedScene defeatScene;

	public override void _Ready()
	{
		base._Ready();
		EnemyManager enemyManager = GetNode<EnemyManager>("EnemyManager");
		enemyManager.GameOver += OnGameOver;
		enemyManager.AllEnemiesAreDead += OnAllEnemiesAreDead;
	}

	private void OnAllEnemiesAreDead()
	{
		Node newVictoryUi = victoryScene.Instantiate();
		AddChild(newVictoryUi);
	}

	private void OnGameOver()
	{
		Node newDefeatUi = defeatScene.Instantiate();
		AddChild(newDefeatUi);
	}
}