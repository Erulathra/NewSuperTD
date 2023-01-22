using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace NewSuperTD.Tiles.Scenes;

[Tool]
public partial class Tile : Node3D
{
	[Export] public Color HoverColor;

	private StandardMaterial3D material;
	[Export] public Color NormalColor;
	private bool isHovered = false;
	private Color modifiersColor = Colors.Transparent;

	[Signal]
	public delegate void InputEventEventHandler(Tile tile, InputEvent inputEvent);

	[Signal]
	public delegate void MouseEnterEventHandler(Tile tile);

	[Signal]
	public delegate void MouseExitEventHandler(Tile tile);

	public Color ModifiersColor
	{
		get => modifiersColor;
		set
		{
			modifiersColor = value;
			UpdateColor();
		}
	}

	public bool IsHovered
	{
		get => isHovered;
		set
		{
			isHovered = value;
			UpdateColor();
		}
	}

	public override void _Ready()
	{
		MeshInstance3D mesh = GetNode<MeshInstance3D>("Mesh");
		material = new StandardMaterial3D();
		material.AlbedoColor = NormalColor;
		mesh.MaterialOverride = material;
	}

	public override void _Process(double delta)
	{
	}

	private void OnArea3dInputEvent(Node camera, InputEvent inputEvent, Vector3 position, Vector3 normal, int shapeIDx)
	{
		EmitSignal(SignalName.InputEvent, this, inputEvent);
	}

	public virtual void OnMouseEntered()
	{
		EmitSignal(SignalName.MouseEnter, this);
	}

	private void OnMouseExited()
	{
		EmitSignal(SignalName.MouseExit, this);
	}

	public List<Tile> GetTilesInRange(int range)
	{
		HashSet<Tile> neighbors = new(GetNeighbours());
		List<Tile> result = new();
		
		if (range == 0)
		{
			result.Add(this);
			return result;
		}

		if (range == 1)
		{
			result = GetNeighbours();
			result.Add(this);
			return result;
		}

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

	public void UpdateColor()
	{
		if (IsHovered)
		{
			material.AlbedoColor = NormalColor.Blend(HoverColor);
		}
		else
		{
			material.AlbedoColor = NormalColor;
		}
		 
		material.AlbedoColor = material.AlbedoColor.Blend(ModifiersColor);
	}
}