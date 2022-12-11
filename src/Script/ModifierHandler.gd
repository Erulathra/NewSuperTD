class_name ModifierHandler
extends Node

signal on_register_modifier(modifier)

var modifiers = []


func _ready():
	pass


func has(modifier_type) -> bool:
	for modifier in modifiers:
		if modifier is modifier_type:
			return true

	return false


func register_modifier(modifier: TileModifier):
	if not modifier in modifiers:
		modifiers.append(modifier)
		modifier.register_callback(get_parent(), get_parent().get_parent())

	if modifier in modifiers:
		emit_signal("on_register_modifier", modifier)

	get_parent().update_color()


func remove_modifiers(modifier_type):
	var remove_indexes = []
	for i in range(modifiers.size()):
		if modifiers[i] is modifier_type:
			remove_indexes.append(i)

	for index in remove_indexes:
		modifiers.remove(index)

	get_parent().update_color()


func unregister_modifier(modifier: TileModifier):
	if modifier in modifiers:
		var index = modifiers.find(modifier)
		modifiers.remove(index)

	get_parent().update_color()


func get_color():
	var result: Color = Color.transparent
	for modifier in modifiers:
		result = result.blend(modifier.color_modifier)

	return result
