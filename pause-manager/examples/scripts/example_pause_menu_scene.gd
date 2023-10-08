extends CanvasLayer


func _on_pause_manager_toggle(paused):
	$"Pause Menu".visible = paused


func _on_pause_button_pressed():
	$PauseManager.call("_toggle_pause")


func _on_resume_button_pressed():
	$PauseManager.call("_toggle_pause")


func _on_main_scene_button_pressed():
	$PauseManager.call("_resume")
	get_tree().change_scene_to_file("res://pause-manager/examples/example_scene.tscn")
