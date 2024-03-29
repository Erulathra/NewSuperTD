using Godot;
using Godot.Collections;
using NewSuperTD.Towers;

namespace NewSuperTD.Tiles.Scenes;

public partial class TowerManager : Node
{

	[Signal] public delegate void TowerPlacedEventHandler(Tower tower);

	private string actualTower = "None";
	private int hoverDistance;

	[Export] public Node TilesParent;
	[Export] public Dictionary<string, Array<Variant>> TowerSceneDictionary;
	public int PlacedTowersCount { get; private set; } = 0;

	// Hack to bypass exported Dictionary godot bug
	private Dictionary<string, Array<Variant>> currentTowerDictionary;

	public Tile LastHoverTile { get; private set; }

	public string ActualTower
	{
		get => actualTower;
		set
		{
			if (value == "None")
			{
				UnHoverTiles(LastHoverTile, hoverDistance);
				actualTower = value;
				hoverDistance = 0;
				LastHoverTile.IsHovered = true;
				return;
			}

			if (!currentTowerDictionary.ContainsKey(value))
			{
				GD.PrintErr("Wrong Tower ID");
				return;
			}

			actualTower = value;
			hoverDistance = GetActualTowerRange();
		}
	}

	public override void _Ready()
	{
		currentTowerDictionary = new();
		foreach (var tower in TowerSceneDictionary)
		{
			currentTowerDictionary.Add(tower.Key, new Array<Variant>(tower.Value));
		}
		
		BindTileInputEvents();
	}

	private void BindTileInputEvents()
	{
		Array<Node> tiles = TilesParent.GetChildren();
		foreach (Node node in tiles)
		{
			Tile tile = node as Tile;
			if (tile == null)
				continue;

			tile.InputEvent += OnTileClick;
			tile.MouseEnter += OnMouseEnter;
			tile.MouseExit += OnMouseExit;
		}
	}

	private void OnMouseEnter(Tile tile)
	{
		LastHoverTile = tile;
		Array<Tile> nearTiles = tile.GetTilesInRange(hoverDistance);
		foreach (Tile nearTile in nearTiles)
			nearTile.IsHovered = true;
	}

	private void OnMouseExit(Tile tile)
	{
		UnHoverTiles(tile, hoverDistance);
	}

	private void UnHoverTiles(Tile tile, int distance)
	{
		Array<Tile> nearTiles = tile.GetTilesInRange(distance);
		foreach (Tile nearTile in nearTiles)
			nearTile.IsHovered = false;
	}

	private void OnTileClick(Tile tile, InputEvent inputEvent)
	{
		InputEventMouseButton mouseButtonEvent = inputEvent as InputEventMouseButton;
		if (mouseButtonEvent == null || mouseButtonEvent.ButtonIndex != MouseButton.Left || !mouseButtonEvent.Pressed)
			return;

		PlaceTower(tile);
	}

	private void PlaceTower(Tile tile)
	{
		if (actualTower == "None")
			return;

		if (tile is PathTile)
			return;

		if ((int)currentTowerDictionary[ActualTower][1] <= 0)
			return;

		PackedScene towerScene = (PackedScene)currentTowerDictionary[ActualTower][0];
		Node3D newTower = towerScene.Instantiate<Node3D>();
		tile.AddChild(newTower);
		newTower.Name = "Tower";
		newTower.Position = tile.GetNode<Node3D>("SurfaceHandle").Position;

		int availableTowers = (int)currentTowerDictionary[ActualTower][1] - 1;
		currentTowerDictionary[ActualTower][1] = availableTowers;

		PlacedTowersCount++;
		EmitSignal(SignalName.TowerPlaced, newTower);
	}

	private int GetActualTowerRange()
	{
		PackedScene towerScene = (PackedScene)currentTowerDictionary[ActualTower][0];
		Tower tower = towerScene.Instantiate<Tower>();
		return tower.Range;
	}
}