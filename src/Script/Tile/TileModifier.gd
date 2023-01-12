class_name TileModifier

var color_modifier: Color


func _init():
	pass


func register_callback(_tile: Tile, _tile_grid):
	pass


func get_damage():
	pass


func animate(tile: Tile):
	var animation_player: AnimationPlayer = tile.get_node("AnimationPlayer")
	animation_player.clear_queue()
	animation_player.stop(true)
	animation_player.play("AreaInRange")