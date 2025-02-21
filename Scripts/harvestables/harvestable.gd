class_name Harvestable extends Object

var gid: int
var name: String
var stage: int
var max_stage: int
var passive_produce: Dictionary
var active_produce: Dictionary

# Does not destroy instance
func harvest() -> Dictionary:
	if self.stage == self.max_stage:
		return self.active_produce
	return {}

func tick() -> void:
	self.stage += 1
