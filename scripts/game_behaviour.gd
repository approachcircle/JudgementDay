extends Node

func _process(delta: float):
	if Input.is_action_just_pressed("exit"):
		get_tree().quit()
