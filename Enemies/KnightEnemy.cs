using System.Linq;
using Godot.Collections;
using NewSuperTD.Tiles.Scenes;

namespace NewSuperTD.Enemies;

public partial class KnightEnemy : Enemy
{
	protected override Tile FindNextTile(int tickCount)
	{
		PathTile parentTile = GetParent<PathTile>();
		Array<Tile> neighbors = parentTile.GetTilesInRange(2);
		neighbors.Remove(parentTile);

		Tile result = null;

		do
		{
			neighbors.Remove(result);

			if (neighbors.Count <= 0)
				return null;
			
			result = neighbors.OfType<PathTile>().MinBy(tile => tile.DistanceToKing);
		} while (!CanMove(result, tickCount));

		return result;
	}
}