@tool
class_name Tile
extends Node3D

@export
var normal_color: Color = Color("#eeedd5")

@export
var hover_color: Color = Color.GRAY

var material: Material


func _ready():
	var mesh: MeshInstance3D = $Mesh
	material = StandardMaterial3D.new()
	material.albedo_color = normal_color
	mesh.material_override = material


func _process(_delta):
	pass


func _on_area_3d_mouse_entered():
	material.albedo_color = normal_color.blend(hover_color)


func _on_area_3d_mouse_exited():
	material.albedo_color = normal_color


func _on_area_3d_input_event(camera, event, position, normal, shape_idx):
	pass # Replace with function body.
