extends TileModifier
class_name ElectricModifier


func _init():
	color_modifier = Color.YELLOW
	color_modifier.a = 0.7


func register_callback(_tile: Tile, _tile_grid):
	var neighbors = _tile_grid.get_neighbour_tiles(_tile.grid_location)
	await _tile.get_tree().create_timer(0.05).timeout

	var unregister_self = func():
		if _tile != null:
			_tile.get_node("ModifierHandler").unregister_modifier(self)

	_tile.get_tree().create_timer(0.2).timeout.connect(unregister_self);

	for neighbor in neighbors:
		if neighbor.get_node("ModifierHandler").has(WaterModifier):
			neighbor.get_node("ModifierHandler").register_modifier(self)
			animate(_tile)


func get_damage():
	return 10;
