using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using NewSuperTD.Towers;

namespace NewSuperTD.Tiles.Scenes;

public partial class TowerManager : Node
{
	[Export()] public Node TilesParent;
	[Export()] public Godot.Collections.Dictionary<String, Array<Variant>> TowerSceneDictionary;

	private Godot.Collections.Dictionary<String, int> usedTowersDictionary;
	private string actualTower = "None";
	private int hoverDistance = 0;
	public String ActualTower
	{
		get => actualTower;
		set
		{
			if (!TowerSceneDictionary.ContainsKey(value))
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
		BindTileInputEvents();
		usedTowersDictionary = new Godot.Collections.Dictionary<string, int>();

		foreach (string towerId in TowerSceneDictionary.Keys)
		{
			usedTowersDictionary.Add(towerId, 0);
		}
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
		List<Tile> nearTiles = tile.GetTilesInRange(hoverDistance);
		foreach (Tile nearTile in nearTiles)
		{
			nearTile.IsHovered = true;
		}
	}

	private void OnMouseExit(Tile tile)
	{
		List<Tile> nearTiles = tile.GetTilesInRange(hoverDistance);
		foreach (Tile nearTile in nearTiles)
		{
			nearTile.IsHovered = false;
		}
	}

	public override void _Process(double delta)
	{
	}

	private void OnTileClick(Tile tile, InputEvent inputEvent)
	{
		if (actualTower == "None")
			return;
		
		InputEventMouseButton mouseButtonEvent = inputEvent as InputEventMouseButton;
		if (mouseButtonEvent == null || mouseButtonEvent.ButtonIndex != MouseButton.Left || !mouseButtonEvent.Pressed)
			return;
		
		if (tile is PathTile)
			return;
		
		if ((int)TowerSceneDictionary[ActualTower][1] <= usedTowersDictionary[ActualTower])
			return;

		PackedScene towerScene = (PackedScene)TowerSceneDictionary[ActualTower][0];
		Node3D newTower = towerScene.Instantiate<Node3D>();
		tile.AddChild(newTower);
		newTower.Name = "Tower";
		newTower.Position = tile.GetNode<Node3D>("SurfaceHandle").Position;

		usedTowersDictionary[ActualTower] += 1;
	}

	private int GetActualTowerRange()
	{
		PackedScene towerScene = (PackedScene)TowerSceneDictionary[ActualTower][0];
		Tower tower = towerScene.Instantiate<Tower>();
		return tower.Range;
	}
	
}
