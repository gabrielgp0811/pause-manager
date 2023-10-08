extends Panel

var speed : float = 2

func _process(delta):
	rotation += speed * delta


func _on_pause_event_handler_toggle(paused):
	print(name, " - PauseEventHandler toggle: ", paused)
	set_process(not paused)


func _on_pause_event_handler_pause():
	print(name, " - PauseEventHandler paused")
	set_process(false)


func _on_pause_event_handler_resume():
	print(name, " - PauseEventHandler resume")
	set_process(true)


func toggle(paused: bool):
	print(name, " - Group toggle: ", paused)
	set_process(not paused)


func pause():
	print(name, " - Group pause")
	set_process(false)


func resume():
	print(name, " - Group resume")
	set_process(true)
