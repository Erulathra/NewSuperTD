extends "res://src/Script/Tile/EndTile.gd"
class_name StartTile


func _ready():
	._ready()


func _process(_delta):
	var target_color = lerp(color_normal, Color.white, sin(OS.get_ticks_msec() / 250.0))
	set_color(target_color)
