extends Camera3D
class_name SuperCamera

@export var NOISE_SHAKE_SPEED: float = 30.0
@export var NOISE_SHAKE_STRENGTH: float = 60.0
@export var SHAKE_DECAY_RATE: float = 3.0

@onready var noise := FastNoiseLite.new()

var noise_i: float = 0.0

@onready var rand = RandomNumberGenerator.new()
var shake_strength: float = 0.0

func _ready() -> void:
	rand.randomize()
	
	# Randomize the generated noise
	noise.seed = rand.randi()
	# Period affects how quickly the noise changes values
	noise.frequency = 2
	
func apply_noise_shake() -> void:
	shake_strength = NOISE_SHAKE_STRENGTH
	
func _process(delta: float) -> void:
	# Fade out the intensity over time
	shake_strength = lerp(shake_strength, 0., SHAKE_DECAY_RATE * delta)
	
	var shake_offset: Vector2
	shake_offset = get_noise_offset(delta, NOISE_SHAKE_SPEED, shake_strength)
	
	# Shake by adjusting camera.offset so we can move the camera around the level via it's position
	self.h_offset = shake_offset.x
	self.v_offset = shake_offset.y

func get_noise_offset(delta: float, speed: float, strength: float) -> Vector2:
	noise_i += delta * speed
	# Set the x values of each call to 'get_noise_2d' to a different value
	# so that our x and y vectors will be reading from unrelated areas of noise
	return Vector2(
		noise.get_noise_2d(1, noise_i) * strength,
		noise.get_noise_2d(100, noise_i) * strength
	)

func get_random_offset() -> Vector2:
	return Vector2(
		rand.randf_range(-shake_strength, shake_strength),
		rand.randf_range(-shake_strength, shake_strength)
	)
