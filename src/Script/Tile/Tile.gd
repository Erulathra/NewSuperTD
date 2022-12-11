extends Spatial
class_name Tile

export var color_normal = Color.white
export var color_hover = Color.gray

var mesh: MeshInstance
var material: SpatialMaterial

var grid_location: Vector2


func _ready():
	mesh = $MeshInstance
	material = SpatialMaterial.new()
	material.albedo_color = color_normal
	mesh.material_override = material


func _on_Area_input_event(
	_camera: Node, _event: InputEvent, _position: Vector3, _normal: Vector3, _shape_idx: int
):
	if _event is InputEventMouseButton:
		if _event.button_index == BUTTON_LEFT and _event.pressed == true:
			on_left_click()


func on_left_click():
	get_parent().click_tile(self)


func _on_Area_mouse_exited():
	material.albedo_color = color_normal.blend($ModifierHandler.get_color())


func _on_Area_mouse_entered():
	material.albedo_color = color_normal * color_hover


func set_color(new_color: Color):
	material.albedo_color = new_color.blend($ModifierHandler.get_color())


func update_color():
	set_color(color_normal)
