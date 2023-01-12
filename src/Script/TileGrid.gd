class_name TileGrid

extends Node3D

signal click_tile(tile)

@export var grid_size: int = 10
@export var grid_spacing: float = 1.2
@export var update_in_editor: bool = false

@export var generator_text = """###e######
###p#ppp##
###p#p#p##
###p#p#p##
###ppp#p##
#######p##
##pppppp##
##p#######
##pppppp##
#######s##"""

const tiles_dict = {
	"#": preload("res://src/Scenes/Tiles/Tile.tscn"),
	"p": preload("res://src/Scenes/Tiles/PathTile.tscn"),
	"s": preload("res://src/Scenes/Tiles/StartTile.tscn"),
	"e": preload("res://src/Scenes/Tiles/EndTile.tscn"),
}

var start_tile: Vector2
var end_tile: Vector2

var tiles_array: Array = []


func _enter_tree():
	clean_grid()
	generate_tiles()


func generate_tiles() -> void:
	var lines = generator_text.split("\n")

	var grid_x: int = 0
	var grid_y: int = 0

	for line in lines:
		var row = []
		for character in line:
			if not character in tiles_dict:
				grid_y += 1
				continue

			var new_tile = tiles_dict[character].instantiate()
			var new_tile_position = Vector3(grid_x * grid_spacing, 0, grid_y * grid_spacing)
			new_tile_position -= Vector3(
				((grid_size - 1) * grid_spacing) / 2, 0, ((grid_size - 1) * grid_spacing) / 2
			)

			new_tile.position = new_tile_position
			new_tile.grid_location = Vector2(grid_x, grid_y)
			add_child(new_tile)
			row.append(new_tile)

			grid_y += 1

		tiles_array.append(row)
		grid_x += 1
		grid_y = 0


func clean_grid() -> void:
	for row in tiles_array:
		for tile in row:
			tile.queue_free()


func get_neighbour_tiles(grid_location: Vector2):
	var result = get_tiles_in_radius_manhatan(grid_location, 1)
	for i in result.size():
		if result[i].grid_location == grid_location:
			result.remove_at(i)
			break

	return result


func get_tiles_in_radius_manhatan(grid_location: Vector2, radius: int):
	var result = []

	for i in range(-radius - 1, radius + 1):
		for j in range(-radius - 1, radius + 1):
			if abs(i) + abs(j) > radius:
				continue

			var target_grid_location = grid_location + Vector2(i, j)
			if not is_grid_location_valid(target_grid_location):
				continue

			result.append(tiles_array[target_grid_location.x][target_grid_location.y])

	return result


func get_tiles_in_radius(grid_location: Vector2, radius: int):
	var result = []

	for i in range(-radius, radius + 1):
		for j in range(-radius, radius + 1):
			var target_grid_location = grid_location + Vector2(i, j)
			if not is_grid_location_valid(target_grid_location):
				continue

			result.append(tiles_array[target_grid_location.x][target_grid_location.y])

	return result


func is_grid_location_valid(grid_location: Vector2) -> bool:
	if grid_location.x >= grid_size or grid_location.y >= grid_size:
		return false

	if grid_location.x < 0 or grid_location.y < 0:
		return false

	return true


func on_click_tile(tile):
	emit_signal("click_tile", tile)
