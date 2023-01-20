class_name PathTile
extends Tile

var distance_to_king = 1024;

# Called when the node enters the scene tree for the first time.
func _ready():
	super._ready()
	
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	super._process(delta)
	pass

func initalize_path():
	var path_tiles = []
	
	var neighbors = get_neighbors()
	for tile in neighbors:
		if tile is PathTile:
			var path_tile = tile as PathTile
			path_tiles.append(path_tile)
			
	if path_tiles.is_empty():
		return
	
	for path_tile in path_tiles:
		if path_tile.distance_to_king < distance_to_king:
			continue
			
		path_tile.distance_to_king = distance_to_king + 1
		path_tile.initalize_path()		
	
