using Godot;

namespace NewSuperTD.Tiles.Scenes;

public partial class KingTile : PathTile
{
	public override void _Ready()
	{
		base._Ready();

		if (Engine.IsEditorHint())
			return;
		
		DistanceToKing = 0;
		InitializePath();
	}
}