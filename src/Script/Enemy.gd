extends Spatial
class_name Enemy

signal on_death(enemy)

export var thinking_time = 1.0
export var moving_speed = 5.0
export var health_points = 100.0

var tile_grid: TileGrid
var actual_tile: PathTile setget set_actual_tile
var previous_tile: PathTile
var thinking_timer: Timer

var target_translation

onready var health_progress_bar: ProgressBar = $HealthBar/Viewport/ProgressBar

var DestroyedEnemyScene = preload("res://src/Scenes/DestroyedEnemyScene.tscn")

func set_actual_tile(new_tile: PathTile):
	previous_tile = actual_tile
	if not previous_tile == null:
		previous_tile.unregister_entity(self)
		previous_tile.get_node("ModifierHandler").disconnect(
			"on_register_modifier", self, "on_register_modifier"
		)

	actual_tile = new_tile
	var _result = actual_tile.get_node("ModifierHandler").connect(
		"on_register_modifier", self, "on_register_modifier"
	)

	actual_tile.register_enemy(self)


func _ready():
	thinking_timer = Timer.new()
	var _return = thinking_timer.connect("timeout", self, "_on_thiniking_timeout")
	thinking_timer.wait_time = thinking_time
	add_child(thinking_timer)
	thinking_timer.start()


func _on_thiniking_timeout():
	think()


func think():
	var next_tile = find_next_path_tile()

	if next_tile is EndTile:
		queue_free()

	move(next_tile)


func _process(delta):
	if target_translation == null:
		global_translation = actual_tile.get_node("TopHandle").global_translation
		target_translation = global_translation

	global_translation = lerp(global_translation, target_translation, delta * moving_speed)

	health_progress_bar.value = lerp(health_progress_bar.value, health_points, delta * 4)


func find_next_path_tile():
	var neighbour_tiles = tile_grid.get_neighbour_tiles(actual_tile.grid_location)

	for tile in neighbour_tiles:
		if tile == previous_tile:
			continue

		if not tile is PathTile:
			continue

		return tile


func move(new_tile: PathTile):
	set_actual_tile(new_tile)
	target_translation = new_tile.get_node("TopHandle").global_translation

	for modifier in actual_tile.get_node("ModifierHandler").modifiers:
		get_damage(modifier.get_damage())


func _exit_tree():
	actual_tile.unregister_entity(self)


func on_register_modifier(modifier):
	get_damage(modifier.get_damage())


func get_damage(amount: float):
	health_points -= amount

	if health_points <= 0:
		var particles = DestroyedEnemyScene.instance()
		get_parent().add_child(particles)
		particles.global_transform = self.global_transform
		
		emit_signal("on_death", self)
		queue_free()
	if amount > 0:
		$Particles.emitting = true

