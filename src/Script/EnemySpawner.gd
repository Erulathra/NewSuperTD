extends Node
class_name EnemySpawner

signal on_spawn_enemy(enemy)
signal on_enemy_death(enemy)
signal on_game_end

@export var enemy_count = 10
var spawned_enemy_count = 0
var enemy_death_count = 0

var tile_grid: TileGrid
var start_tile: StartTile


func _ready():
	tile_grid = get_parent()
	if not tile_grid is TileGrid:
		push_error("Parent is not a TileGrid")
		return

	if not find_start_tile():
		return

	var spawn_timer: Timer = $SpawnTimer
	spawn_timer.start()


func find_start_tile() -> bool:
	for row in tile_grid.tiles_array:
		for tile in row:
			if tile is StartTile:
				start_tile = tile as StartTile
				return true

	push_error("There is not any start tiles")
	return false


var enemy_scene = preload("res://src/Scenes/Enemy.tscn")


func _on_SpawnTimer_timeout():
	if enemy_count <= spawned_enemy_count:
		return

	var new_enemy: Enemy = enemy_scene.instantiate()
	add_child(new_enemy)
	
	new_enemy.position = start_tile.position
	new_enemy.actual_tile = start_tile
	new_enemy.tile_grid = tile_grid
	var _result = new_enemy.connect("on_death",Callable(self,"_on_enemy_death"))

	emit_signal("on_spawn_enemy", new_enemy)

	spawned_enemy_count += 1


func _on_enemy_death(enemy):
	emit_signal("on_enemy_death", enemy)
	enemy_death_count += 1

	if enemy_death_count >= enemy_count:
		emit_signal("on_game_end")
