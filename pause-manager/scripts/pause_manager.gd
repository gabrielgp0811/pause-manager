@icon("res://pause-manager/icon.png")

class_name PauseManager
extends Node
## A simple package for managing pause/resume using Signals.
## 
## Author: Gabriel Pereira
## E-mail: gabrielgp0811@gmail.com
## Github: https://github.com/gabrielgp0811
## Discord: gabrielgp0811 | ggpereira#6599

# SIGNALS
signal toggle(paused: bool)
signal pause
signal resume

# CONSTANTS
const VERSION = "1.0.0"

# EXPORTED VARIABLES

## When 'true', the property 'get_tree().paused' will be changed to 'true' when paused
## and changed to 'false' when resumed.
@export var pause_tree: bool = true

## When 'true', the 'pause' and 'resume' functions defined in all Nodes
## belonging from 'groups' Array below will be triggered.
@export var pause_groups: bool = true

## Array of groups to pause/resume a Node.
## Used when 'pause_tree' is 'false'.
@export var groups: Array[StringName] = ["pausable"]

## When 'true', the 'pause' and 'resume' functions from 'PauseEventHandler' will be triggered.
@export var use_event_handlers: bool = true

## Use actions from Input Map? (Project -> Project Settings -> Input Map).
@export var use_input_map: bool = true

## Action names from Input Map (Project -> Project Settings -> Input Map).
## By default, there's only one action with the name 'pause'.
@export var action_names: Array[StringName] = ["pause"]

## Array of keys, when Input Map is not needed.
## By default, there's only the 'Escape' key from keyboard.
@export var keys: Array[Key] = [KEY_ESCAPE]

# PRIVATE VARIABLES
var _paused: bool = false
var _input_event_pressed: bool = false

func _process(_delta):
	if use_input_map:
		for action_name in action_names:
			if Input.is_action_just_pressed(action_name):
				_toggle_pause()

func _input(event):
	if not use_input_map and event is InputEventKey:
		if event.pressed:
			if not _input_event_pressed:
				_input_event_pressed = true
				_toggle_pause()
		else:
			_input_event_pressed = false

func _notification(what):
	if what == NOTIFICATION_WM_WINDOW_FOCUS_OUT:
		_pause()

func _toggle_pause() -> void:
	if not is_paused():
		_pause()
	else:
		_resume()

func _pause() -> void:
	if is_paused():
		return
	
	set_paused(true)
	
	toggle.emit(true)
	pause.emit()
	
	if pause_tree:
		get_tree().paused = true
	else:
		if pause_groups:
			for group in groups:
				get_tree().call_group(group, "toggle", true)
				get_tree().call_group(group, "pause")
		if use_event_handlers:
			emit_handlers(get_tree().root, true)

func _resume() -> void:
	if not is_paused():
		return
	
	set_paused(false)
	
	toggle.emit(false)
	resume.emit()
	
	get_tree().paused = false
	
	if not pause_tree:
		if pause_groups:
			for group in groups:
				get_tree().call_group(group, "toggle", false)
				get_tree().call_group(group, "resume")
		if use_event_handlers:
			emit_handlers(get_tree().root, false)

func emit_handlers(node : Node, pause : bool = true) -> void:
	var pause_event_handler_node
	for child in node.get_children():
		pause_event_handler_node = child.get_node_or_null("PauseEventHandler")
		
		if pause_event_handler_node != null:
			pause_event_handler_node.emit_toggle(pause)
			if pause:
				pause_event_handler_node.emit_pause()
			else:
				pause_event_handler_node.emit_resume()
		
		emit_handlers(child, pause)

func append_action(action: StringName) -> void:
	if not action_names.has(action):
		action_names.append(action)

func remove_action(action: StringName) -> void:
	remove_action_at(action_names.find(action))

func remove_action_at(position: int) -> void:
	if position > 0 and position < action_names.size():
		action_names.remove_at(position)

func set_paused(value: bool) -> void:
	_paused = value

func is_paused() -> bool:
	return _paused

func set_pause_tree(value: bool):
	pause_tree = value

func is_pause_tree():
	return pause_tree

func set_pause_groups(value: bool) -> void:
	if is_paused():
		return
	
	pause_groups = value

func is_pause_groups() -> bool:
	return pause_groups

func set_use_event_handlers(value: bool) -> void:
	if is_paused():
		return
	
	use_event_handlers = value

func is_use_event_handlers() -> bool:
	return use_event_handlers

