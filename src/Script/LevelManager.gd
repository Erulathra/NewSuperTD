class_name LevelManager
extends Node

var current_level = "levelOne"
const levels = {"levelOne": "res://src/Scenes/Levels/LevelOne.tscn"}


# Called when the node enters the scene tree for the first time.
func _ready():
	pass  # Replace with function body.


func restart_level():
	var scene_transition: SceneTransition = $SceneTransition
	#scene_transition.StartEndSceneAnimation()

	var level = get_parent().find_child("Level", true)
	level.queue_free()

	var new_level_instance = load(levels[current_level]).instantiate()
	get_parent().add_child(new_level_instance)

	current_level = new_level_instance.level_name

	#scene_transition.StartStartSceneAnimation()
