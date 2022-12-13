extends PathTile
class_name StartTile


func _ready():
	super._ready()


func _process(_delta):
	var target_color = lerp(color_normal, Color.WHITE, sin(Time.get_ticks_msec() / 250.0))
	set_color(target_color)
