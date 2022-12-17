extends Tower
class_name WaterTower
var modifier: TileModifier = WaterModifier.new()


func _ready():
	super._ready()
	tiles_in_range_array = tile_grid.get_tiles_in_radius_manhatan(get_parent().grid_location, 1)

	think()


func think():
	super.think()

	for tile in tiles_in_range_array:
		tile.get_node("ModifierHandler").register_modifier(modifier)
	animate()
