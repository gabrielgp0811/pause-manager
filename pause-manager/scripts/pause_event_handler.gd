@icon("res://pause-manager/icon.png")

class_name PauseEventHandler
extends Node
## Used for handling pause/resume of a single Node
## 
## Author: Gabriel Pereira
## E-mail: gabrielgp0811@gmail.com
## Github: https://github.com/gabrielgp0811
## Discord: gabrielgp0811 | ggpereira#6599

# SIGNALS
signal toggle(paused: bool)
signal pause
signal resume

func emit_toggle(paused: bool):
	toggle.emit(paused)

func emit_pause():
	pause.emit()

func emit_resume():
	resume.emit()
