using Godot;
using Godot.Collections;
using NewSuperTD.Tiles.Scenes;
using NewSuperTD.Towers;
using NewSuperTD.UI.TowerCards;

public partial class GameHud : Control
{
	[Export] private Dictionary<string, PackedScene> towerCardsDictionary = new();
	private Array<TowerCard> cards = new();
	private TowerCard selectedCard;
	
	private TowerManager towerManager;
	private HBoxContainer hBox;
	
	public override void _Ready()
	{
		base._Ready();
		OnLevelLoad();
	}

	public void OnLevelLoad()
	{
		towerManager = GetParent().GetNode<TowerManager>("TowerManager");
		towerManager.TowerPlaced += OnTowerPlaced;
		
		hBox = GetNode<HBoxContainer>("CanvasLayer/CardHorizontalBox");

		Dictionary<string, Array<Variant>> towerSceneDictionary = towerManager.TowerSceneDictionary;
		foreach (var tower in towerSceneDictionary)
		{
			int availableTowers = (int)tower.Value[1];
			
			for (int i = 0; i < availableTowers; i++)
			{
				if (!towerCardsDictionary.ContainsKey(tower.Key))
				{
					GD.PushError($"No card in dictionary with id: {tower.Key}");
					return;
				}
				
				PackedScene cardScene = towerCardsDictionary[tower.Key];
				TowerCard newCard = (TowerCard)cardScene.Instantiate();
				newCard.TowerId = tower.Key;
				
				cards.Add(newCard);
				hBox.AddChild(newCard);

				newCard.ButtonUp += OnCardUp;
			}
		}
	}

	private void OnTowerPlaced(Tower tower)
	{
		towerManager.ActualTower = "None";
		cards.Remove(selectedCard);
		selectedCard.QueueFree();
		selectedCard = null;
	}

	private void OnCardUp(string towerId, TowerCard towerCard)
	{
		foreach (var card in cards)
		{
			card.UnSelect();
		}
		
		towerCard.Select();
		selectedCard = towerCard;

		towerManager.ActualTower = towerId;
	}


	private void SetTower(string towerId)
	{
		towerManager.ActualTower = towerId;
	}
}