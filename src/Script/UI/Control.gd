extends Control

var tower_manager: TowerManager
var enemy_spawner: EnemySpawner
var level_manager: LevelManager

var score := 0

@onready
var towers_amount_labels = {
	"Water": $CanvasLayer/HBoxContainer/WaterButton/Amount,
	"Fire": $CanvasLayer/HBoxContainer/FireButton/Amount,
	"Electric": $CanvasLayer/HBoxContainer/ElectricButton/Amount
}


func _ready():
	level_manager = get_tree().get_root().find_child("LevelManager", true, false)
	level_manager.on_level_loaded.connect(on_level_loaded)
	on_level_loaded()
	update_towers_amount()


func on_level_loaded():
	tower_manager = get_tree().get_root().find_child("TowerManager", true, false)
	enemy_spawner = get_tree().get_root().find_child("EnemySpawner", true, false)
	update_towers_amount()
	enemy_spawner.on_enemy_death.connect(update_score)
	tower_manager.on_placed_tower.connect(update_towers_amount)


func update_score(_enemy):
	score += 100
	$CanvasLayer/Score.set_text("[right]Score %d[/right]" % score)


func update_towers_amount():
	for tower in towers_amount_labels:
		var label = towers_amount_labels[tower]
		var placed_towers = tower_manager.placed_towers_dict[tower]
		var avialable_towers = tower_manager.tower_amount[tower]

		label.set_text("{0}".format([avialable_towers - placed_towers]))


func _on_fire_button_pressed():
	tower_manager.change_tower("Fire")


func _on_water_button_pressed():
	tower_manager.change_tower("Water")


func _on_electric_button_pressed():
	tower_manager.change_tower("Electric")
