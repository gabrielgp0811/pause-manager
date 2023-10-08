extends CanvasLayer

func _ready():
	$PauseTreeCheckBox.button_pressed = $PauseManager.call("is_pause_tree")
	$PauseGroupCheckBox.button_pressed = $PauseManager.call("is_pause_groups")
	$PauseEventHandlerCheckBox.button_pressed = $PauseManager.call("is_use_event_handlers")


func _on_pause_tree_check_box_toggled(button_pressed):
	$PauseManager.call("set_pause_tree", button_pressed)


func _on_pause_group_check_box_toggled(button_pressed):
	$PauseManager.call("set_pause_groups", button_pressed)


func _on_pause_event_handler_check_box_toggled(button_pressed):
	$PauseManager.call("set_use_event_handlers", button_pressed)


func _on_change_scene_button_pressed():
	get_tree().change_scene_to_file("res://pause-manager/examples/example_pause_menu_scene.tscn")


func _on_pause_manager_toggle(paused):
	$PauseLabel.visible = paused
