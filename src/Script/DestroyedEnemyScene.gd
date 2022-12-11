extends Node3D

func _ready():
	$GPUParticles3D.emitting = true
	$Particles2.emitting = true

func _on_Timer_timeout():
	queue_free()
