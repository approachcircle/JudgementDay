extends Control

func _ready() -> void:
	get_node("Button").connect("pressed",  func on_pressed(): get_tree().change_scene_to_file("res://scenes/MainScene.tscn"))
