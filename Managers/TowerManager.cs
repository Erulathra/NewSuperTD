using Godot;
using System;
using Godot.Collections;

namespace NewSuperTD.Tiles.Scenes;

public partial class TowerManager : Node
{
	[Export()] public Node TilesParent;
	[Export()] public PackedScene TowerScene;
	public override void _Ready()
	{
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
		}
	}

	public override void _Process(double delta)
	{
	}

	private void OnTileClick(Tile tile, InputEvent inputEvent)
	{
		InputEventMouseButton mouseButtonEvent = inputEvent as InputEventMouseButton;
		if (mouseButtonEvent == null || mouseButtonEvent.ButtonIndex != MouseButton.Left || !mouseButtonEvent.Pressed)
			return;
		
		if (tile is PathTile)
			return;
		
		if (tile.HasNode("Tower"))
			return;

		Node3D newTower = (Node3D)TowerScene.Instantiate();
		tile.AddChild(newTower);
		newTower.Name = "Tower";
		newTower.Position = tile.GetNode<Node3D>("SurfaceHandle").Position;
	}
	
}
