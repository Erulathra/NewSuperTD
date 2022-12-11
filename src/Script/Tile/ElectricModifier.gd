extends TileModifier
class_name ElectricModifier


func _init():
	color_modifier = Color.YELLOW
	color_modifier.a = 0.7


func register_callback(_tile: Tile, _tile_grid):
	var neighbors = _tile_grid.get_neighbour_tiles(_tile.grid_location)

	for neighbor in neighbors:
		if neighbor.get_node("ModifierHandler").has(WaterModifier):
			neighbor.get_node("ModifierHandler").register_modifier(self)

	await _tile.get_tree().create_timer(0.2).timeout
	_tile.get_node("ModifierHandler").unregister_modifier(self)


func get_damage():
	return 10;
