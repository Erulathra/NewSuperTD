extends TileModifier
class_name WaterModifier


func _init():
	color_modifier = Color.cyan
	color_modifier.a = 0.7


func register_callback(_tile: Tile, _tile_grid):
	pass


func get_damage():
	return 0
