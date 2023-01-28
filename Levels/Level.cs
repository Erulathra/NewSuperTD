using Godot;
using NewSuperTD.Managers;

namespace NewSuperTD.Levels;

public partial class Level : Node3D
{
	[Export] public int TowersToThreeStars { get; private set; }
	[Export] public int TowersToTwoStars { get; private set; }

	public override void _Ready()
	{
		base._Ready();
		EnemyManager enemyManager = GetNode<EnemyManager>("EnemyManager");
		enemyManager.EnemyReachTarget += OnEnemyReachTarget;
		enemyManager.AllEnemiesAreDead += OnAllEnemiesAreDead;
	}

	private void OnAllEnemiesAreDead()
	{
		throw new System.NotImplementedException();
	}

	private void OnEnemyReachTarget()
	{
		throw new System.NotImplementedException();
	}
}