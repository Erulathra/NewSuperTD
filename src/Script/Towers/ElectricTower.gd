extends Tower
class_name ElectricTower

var modifier: TileModifier = ElectricModifier.new()


func _ready():
	super._ready()
	pass


func think():
	get_parent().get_node("ModifierHandler").register_modifier(modifier)
	animate();
