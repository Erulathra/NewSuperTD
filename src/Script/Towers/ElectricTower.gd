extends Tower
class_name ElectricTower

var modifier: TileModifier = ElectricModifier.new()


func _ready():
	super._ready()
	tiles_in_range_array = [get_parent()]

	pass


func think():
	super.think()

	get_parent().get_node("ModifierHandler").register_modifier(modifier)
	animate()
