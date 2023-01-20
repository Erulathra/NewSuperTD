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
	if self is PathTile:
		return
	
	for neighbor in get_neighbors_in_range(2):
		if neighbor is Tile:
			neighbor.material.albedo_color = neighbor.normal_color.blend(neighbor.hover_color)



func _on_area_3d_mouse_exited():
	for neighbor in get_neighbors_in_range(2):
		if neighbor is Tile:
			neighbor.material.albedo_color = neighbor.normal_color


func _on_area_3d_input_event(_camera, _event, _position, _normal, _shape_idx):
	if _event is InputEventMouseButton:
		if self is PathTile:
			var self_as_path = self as PathTile
			print(self_as_path.distance_to_king)

func get_neighbors_in_range(range):
	if range <= 1:
		var result = get_neighbors()
		result.append(self)
		return result
	
	var result = []
	for neighbor in get_neighbors():
		for distant_neighbor in neighbor.get_neighbors_in_range(range - 1):
			if not result.has(distant_neighbor):
				result.append(distant_neighbor)
				
	return result
	

func get_neighbors():
	var result = []

	var space_state = get_world_3d().direct_space_state
	if space_state == null:
		return []

	var global_basis = global_transform.basis;
	for vector in [global_basis.x, -global_basis.x, global_basis.z, -global_basis.z]:
		var raycast_query = PhysicsRayQueryParameters3D.create(position, position + vector * 0.5)
		raycast_query.collide_with_areas = true
		var raycast_result = space_state.intersect_ray(raycast_query)
		if not raycast_result.is_empty():
			result.append(raycast_result.collider.get_node("../../"))
		
	return result;
