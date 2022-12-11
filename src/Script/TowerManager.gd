extends Node
class_name TowerManager

signal tower_type_change(tower_name)

var tile_grid: TileGrid

var towers_placed = 0

var towers_dict = {
	1: [preload("res://src/Scenes/Towers/WaterTower.tscn"), 1],
	2: [preload("res://src/Scenes/Towers/ElectricTower.tscn"), 1],
	3: [preload("res://src/Scenes/Towers/FireTower.tscn"), 2]
}

var placed_towers_dict = {
	1: 0,
	2: 0,
	3: 0
}

var active_tower: int = 1


func _ready():
	tile_grid = get_parent()
	if not tile_grid is TileGrid:
		push_error("Parent is not a TileGrid")
		return

	var _result = tile_grid.connect("click_tile",Callable(self,"spawn_tower"))


func spawn_tower(tile: Tile):
	if tile.has_node("Tower"):
		return
		
	if placed_towers_dict[active_tower] >= towers_dict[active_tower][1]:
		return
		
	var new_tower: Node3D = towers_dict[active_tower][0].instantiate()
	new_tower.tile_grid = tile.get_parent()
	tile.add_child(new_tower)
	new_tower.position = tile.get_node("TopHandle").position
	towers_placed += 1
	placed_towers_dict[active_tower] += 1
	

func _process(_delta):
	if Input.is_key_pressed(KEY_1):
		active_tower = 1
		emit_signal("tower_type_change", "Water")
	elif Input.is_key_pressed(KEY_2):
		active_tower = 2
		emit_signal("tower_type_change", "Electric")
	elif Input.is_key_pressed(KEY_3):
		active_tower = 3
		emit_signal("tower_type_change", "Fire")
