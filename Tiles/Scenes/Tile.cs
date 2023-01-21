using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace NewSuperTD.Tiles.Scenes;

[Tool]
public partial class Tile : Node3D
{
	[Export] public Color HoverColor = Colors.Gray;

	private StandardMaterial3D material;
	[Export] public Color NormalColor = Color.FromString("#eeedd5", Colors.White);

	public override void _Ready()
	{
		MeshInstance3D mesh = GetNode<MeshInstance3D>("Mesh");
		material = new StandardMaterial3D();
		material.AlbedoColor = NormalColor;
		mesh.MaterialOverride = material;
	}

	public override void _Process(double delta)
	{ }

	private void OnArea3dInputEvent(Node camera, InputEvent inputEvent, Vector3 position, Vector3 normal, int shapeIDx)
	{
		if (inputEvent is InputEventMouseButton)
			GD.Print("Click!");
	}

	public virtual void OnMouseEntered()
	{
		List<Tile> tiles = GetTilesInRange(2);
		foreach (Tile tile in tiles)
		{
			if (tile == null)
				continue;

			tile.material.AlbedoColor = tile.NormalColor.Blend(tile.HoverColor);
		}
	}

	private void OnMouseExited()
	{
		List<Tile> tiles = GetTilesInRange(2);
		foreach (Tile tile in tiles)
		{
			if (tile == null)
				continue;

			tile.material.AlbedoColor = tile.NormalColor;
		}
	}

	public List<Tile> GetTilesInRange(int range)
	{
		HashSet<Tile> neighbors = new(GetNeighbours());
		if (range <= 1)
		{
			neighbors.Add(this);
			return neighbors.ToList();
		}

		HashSet<Tile> result = new();
		foreach (Tile neighbor in neighbors)
		{
			List<Tile> distantNeighbors = neighbor.GetTilesInRange(range - 1);
			foreach (Tile distantNeighbor in distantNeighbors) result.Add(distantNeighbor);
		}

		return result.ToList();
	}

	public List<Tile> GetNeighbours()
	{
		List<Tile> result = new();

		PhysicsDirectSpaceState3D spaceState = GetWorld3d().DirectSpaceState;
		if (spaceState == null)
			return result;

		Basis globalBasis = GlobalTransform.basis;
		Vector3[] worldDirections = { globalBasis.x, -globalBasis.x, globalBasis.z, -globalBasis.z };
		foreach (Vector3 direction in worldDirections)
		{
			PhysicsRayQueryParameters3D raycastQuery = PhysicsRayQueryParameters3D.Create(Position, Position + 0.5f * direction);
			raycastQuery.CollideWithAreas = true;
			Dictionary raycastResult = spaceState.IntersectRay(raycastQuery);
			if (raycastResult.Count > 0)
			{
				Node collider = (Node)raycastResult["collider"];
				result.Add(collider.GetOwner<Tile>());
			}
		}

		return result;
	}
}