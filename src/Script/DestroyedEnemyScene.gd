extends Node3D


func _ready():
	$SFX.play()


func _on_Timer_timeout():
	pass


func _on_play_timer_timeout():
	$Particles1.emitting = true
	$Particles2.emitting = true
