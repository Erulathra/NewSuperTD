using Godot.Collections;

namespace NewSuperTD.Tiles.Scenes;

public partial class PathTile : Tile
{
	public int DistanceToKing { get; protected set; } = 1024;

	public override void OnMouseEntered()
	{
		return;
	}

	public void InitializePath()
	{
		Array<PathTile> pathTiles = new();

		Array<Tile> neighbors = GetNeighbors();
		foreach (Tile tile in neighbors)
		{
			PathTile pathTile = tile as PathTile;
			if (pathTile != null)
				pathTiles.Add(pathTile);
		}

		if (pathTiles.Count <= 0)
			return;

		foreach (PathTile pathTile in pathTiles)
		{
			if (pathTile.DistanceToKing < DistanceToKing)
				continue;

			pathTile.DistanceToKing = DistanceToKing + 1;
			pathTile.InitializePath();
			return;
		}
	}
}