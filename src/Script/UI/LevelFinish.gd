extends Control
class_name LevelFinish

const text = "[center][color=#ffff00][wave] {stars} [/wave][/color][/center]"


func init():
	pass


func _ready():
	pass


func set_score(score):
	var stars_string = ""
	if score >= 2000:
		stars_string = "  "
	elif score > 800:
		stars_string = " "
	elif score > 0:
		stars_string = ""

	$CanvasLayer/Panel/VBoxContainer/StarLabel.text = text.format({"stars": stars_string})


func _on_button_pressed():
	var level_manager: LevelManager = get_tree().current_scene.find_child("LevelManager", true)
	level_manager.restart_level()
