class_name SceneTransition
extends Control


func _ready():
	pass  # Replace with function body.


func _process(_delta):
	pass


func StartEndSceneAnimation():
	$AnimationPlayer.play("LoadScene")


func StartStartSceneAnimation():
	$AnimationPlayer.clear_queue()
	$AnimationPlayer.stop()
	$AnimationPlayer.play("UnloadScene")
