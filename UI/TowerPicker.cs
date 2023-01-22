using Godot;
using NewSuperTD.Tiles.Scenes;

public partial class TowerPicker : Control
{

	private void SetTower(string towerId)
	{
		TowerManager towerManager = (TowerManager)GetTree().Root.FindChild("TowerManager", true, false);
		towerManager.ActualTower = towerId;
	}
}