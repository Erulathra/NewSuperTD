class_name SceneTransition
extends Control


func _ready():
	pass  # Replace with function body.


func _process(_delta):
	pass


func StartEndSceneAnimation():
	$AnimationPlayer.play("LoadScene")
	await $AnimationPlayer.animation_finished


func StartStartSceneAnimation():
	$AnimationPlayer.play("UnloadScene")
	await $AnimationPlayer.animation_finished
