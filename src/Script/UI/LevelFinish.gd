extends Control
class_name LevelFinish

const text = "[center][color=#ffff00][wave] {stars} [/wave][/color][/center]"


func set_score(score):
	var stars_string = ""
	if score >= 2000:
		stars_string = "  "
	elif score > 800:
		stars_string = " "
	elif score > 0:
		stars_string = ""

	$CanvasLayer/VBoxContainer/StarLabel.text = text.format({"stars": stars_string})
