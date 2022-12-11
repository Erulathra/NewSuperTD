extends Node3D
class_name Tower

@export var thinking_time := 1.0

var tile_grid: TileGrid
var tiles_in_range_array

var thinking_timer: Timer


func _ready():
	thinking_timer = Timer.new()
	var _return = thinking_timer.connect("timeout",Callable(self,"_on_thinking_timeout"))
	thinking_timer.wait_time = thinking_time
	add_child(thinking_timer)
	thinking_timer.start()


func _on_thinking_timeout():
	think()


func think():
	pass

func animate():
	var animation_player = $AnimationPlayer
	animation_player.play("SquashAnimation")

		
			
