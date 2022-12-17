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
	if tiles_in_range_array == null:
		return

	for tile in tiles_in_range_array:
		var animation_player: AnimationPlayer = tile.get_node("AnimationPlayer")
		animation_player.clear_queue()
		animation_player.stop(true)
		animation_player.play("AreaInRange")

func animate():
	animation_player.play("SquashAnimation")
