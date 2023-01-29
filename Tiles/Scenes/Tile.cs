using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

namespace NewSuperTD.Tiles.Scenes;

[Tool]
public partial class Tile : Node3D
{
	[Signal] public delegate void InputEventEventHandler(Tile tile, InputEvent inputEvent);
	[Signal] public delegate void MouseEnterEventHandler(Tile tile);
	[Signal] public delegate void MouseExitEventHandler(Tile tile);

	[Export] public Color HoverColor = Color.Color8(0, 0, 0, 80);
	[Export] public Color NormalColor = Color.FromString("#eeedd5", Colors.White);
	
	private bool isHovered;

	private StandardMaterial3D material;
	private Color modifiersColor = Colors.Transparent;
	private Array<Tile> neighbors;

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

		if (Engine.IsEditorHint())
			return;
		
		neighbors = RaycastNeighbors();
	}

	public override void _Process(double delta)
	{ }

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

	public Array<Tile> GetTilesInRange(int range)
	{

		if (range == 0)
		{
			Array<Tile> resultArray = new Array<Tile>();
			resultArray.Add(this);
			return resultArray;
		}

		if (range == 1)
		{
			Array<Tile> resultArray = GetNeighbors();
			resultArray.Add(this);
			return resultArray;
		}
		
		HashSet<Tile> result = new();

		Array<Tile> tempNeighbors = GetNeighbors();
		foreach (Tile neighbor in tempNeighbors)
		{
			Array<Tile> distantNeighbors = neighbor.GetTilesInRange(range - 1);
			foreach (Tile distantNeighbor in distantNeighbors)
				result.Add(distantNeighbor);
		}

		return new Array<Tile>(result);
	}

	public Array<Tile> GetNeighbors()
	{
		neighbors ??= RaycastNeighbors();

		return new Array<Tile>(neighbors);
	}

	private Array<Tile> RaycastNeighbors()
	{
		Array<Tile> result = new();

		PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;

		Basis globalBasis = GlobalTransform.Basis;
		Vector3[] worldDirections = { globalBasis.X, -globalBasis.X, globalBasis.Z, -globalBasis.Z };
		foreach (Vector3 direction in worldDirections)
		{
			PhysicsRayQueryParameters3D raycastQuery = PhysicsRayQueryParameters3D.Create(Position, Position + 0.5f * direction);
			raycastQuery.CollideWithAreas = true;
			Dictionary raycastResult = spaceState.IntersectRay(raycastQuery);

			if (raycastResult.Count <= 0)
				continue;

			Node collider = (Node)raycastResult["collider"];
			Tile tile = collider.GetOwner<Tile>();

			if (tile == null)
				continue;

			result.Add(collider.GetOwner<Tile>());
		}

		return result;
	}

	private void UpdateColor()
	{
		material.AlbedoColor = IsHovered ? NormalColor.Blend(HoverColor) : NormalColor;
		material.AlbedoColor = material.AlbedoColor.Blend(ModifiersColor);
	}
}