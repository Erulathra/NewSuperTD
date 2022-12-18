class_name LevelManager
extends Node

var current_level = "levelOne"
const levels = {"levelOne": "res://src/Scenes/Levels/LevelOne.tscn"}


# Called when the node enters the scene tree for the first time.
func _ready():
	pass  # Replace with function body.


func restart_level():
	var scene_transition: AnimationPlayer = $SceneTransition/AnimationPlayer
	scene_transition.clear_queue()
	scene_transition.play("UnloadScene")
	await scene_transition.animation_finished

	var currentLevel = get_parent().get_node("Level")
	currentLevel.remove_child(currentLevel)
	currentLevel.queue_free()

	await get_tree().process_frame

	var new_level_instance = load(levels[current_level]).instantiate()
	get_parent().add_child(new_level_instance)
	new_level_instance.name = "Level"

	current_level = new_level_instance.level_name

	scene_transition.play("LoadScene")
	await scene_transition.animation_finished
