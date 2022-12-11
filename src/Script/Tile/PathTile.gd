extends Tile
class_name PathTile

var enemies = []


func _ready():
	._ready()


func on_left_click():
	pass


func register_enemy(enemy):
	if not enemy in enemies:
		enemies.append(enemy)


func unregister_entity(enemy):
	if enemy in enemies:
		var index = enemies.find(enemy)
		enemies.remove(index)
