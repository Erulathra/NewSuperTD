extends Node

var enemy_spawner
var tower_manager
var camera : SuperCamera

var level_finish_ui = preload("res://src/Scenes/UI/LevelFinish.tscn")

var score = 0


func _ready():
	enemy_spawner = get_node("../EnemySpawner")
	tower_manager = get_node("../TowerManager")
	camera = get_node("/root").find_child("Camera3D", true, false)
	
	enemy_spawner.connect("on_game_end",Callable(self,"on_game_end"))
	enemy_spawner.connect("on_enemy_death",Callable(self,"update_score"))
	enemy_spawner.connect("on_enemy_death",Callable(self,"shake_camera"))


func on_game_end():
	var finish_ui = level_finish_ui.instantiate()

	if tower_manager.towers_placed < 3:
		finish_ui.set_score(score + 1000)
	else:
		finish_ui.set_score(score)

	add_child(finish_ui)


func update_score(_enemy):
	score += 100
	
func shake_camera(_enemy):
	camera.apply_noise_shake()
