extends Node
class_name TowerManager

signal tower_type_change(tower_name)
signal on_placed_tower()

var tile_grid: TileGrid

var towers_placed = 0

@export
var tower_amount = {
	'Water': 1,
	'Electric': 1,
	'Fire': 2
}

var towers_dict = {
	'Water': preload("res://src/Scenes/Towers/WaterTower.tscn"),
	'Electric': preload("res://src/Scenes/Towers/ElectricTower.tscn"),
	'Fire': preload("res://src/Scenes/Towers/FireTower.tscn")
}

var placed_towers_dict = {'Water': 0, 'Electric': 0, 'Fire': 0}

var active_tower: String = 'None'


func _ready():
	tile_grid = get_parent()
	if not tile_grid is TileGrid:
		push_error("Parent is not a TileGrid")
		return

	var _result = tile_grid.connect("click_tile", Callable(self, "spawn_tower"))


func spawn_tower(tile: Tile):
	if tile.has_node("Tower"):
		return
		
	if not active_tower in towers_dict.keys():
		return

	if placed_towers_dict[active_tower] >= tower_amount[active_tower]:
		return

	var new_tower: Node3D = towers_dict[active_tower].instantiate()
	new_tower.tile_grid = tile.get_parent()
	tile.add_child(new_tower)
	new_tower.position = tile.get_node("Mesh/TopHandle").position
	towers_placed += 1
	placed_towers_dict[active_tower] += 1
	on_placed_tower.emit()


func change_tower(tower_type):
	if not tower_type in towers_dict.keys():
		return
	
	print(tower_type)

	active_tower = tower_type
	tower_type_change.emit(tower_type)
