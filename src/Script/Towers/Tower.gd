extends Node3D
class_name Tower

var tile_grid: TileGrid
var tiles_in_range_array

@onready var thinking_timer = $ThinkingTimer
@onready var animation_player = $FX/AnimationPlayer

func _ready():
	thinking_timer.start()

func _on_thinking_timer_timeout():
	think()

func think():
	pass

func animate():
	animation_player.play("SquashAnimation")
