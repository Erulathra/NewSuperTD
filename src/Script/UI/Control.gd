extends Control

var tower_manager: TowerManager
var enemy_spawner: EnemySpawner

var score := 0


func _ready():
	tower_manager = get_node("../TileGrid/TowerManager")
	enemy_spawner = get_node("../TileGrid/EnemySpawner")
	var _result = tower_manager.connect("tower_type_change",Callable(self,"tower_type_change"))
	_result = enemy_spawner.connect("on_enemy_death",Callable(self,"update_score"))


func update_score(_enemy):
	score += 100
	$CanvasLayer/Score.set_text("Score %d" % score)


func tower_type_change(tower_name):
	if tower_name == "Water":
		$CanvasLayer/TowerName.set_text("> 1. Water \n2. Electric\n3. Fire")
	if tower_name == "Electric":
		$CanvasLayer/TowerName.set_text("1. Water \n> 2. Electric\n3. Fire")
	if tower_name == "Fire":
		$CanvasLayer/TowerName.set_text("1. Water \n2. Electric\n> 3. Fire")
