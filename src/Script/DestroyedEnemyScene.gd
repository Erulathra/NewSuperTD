extends Spatial

func _ready():
	$Particles.emitting = true
	$Particles2.emitting = true

func _on_Timer_timeout():
	queue_free()
