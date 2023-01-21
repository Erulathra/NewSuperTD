namespace NewSuperTD.Tiles.Scenes;

public partial class KingTile : PathTile
{
	public override void _Ready()
	{
		base._Ready();
		DistanceToKing = 0;
		InitializePath();
	}
}