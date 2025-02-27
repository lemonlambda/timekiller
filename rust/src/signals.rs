use godot::classes::*;
use godot::global::*;
use godot::prelude::*;

#[derive(GodotClass)]
#[class(init, base = Node)]
pub struct Signals {
    base: Base<Node>,
}

#[godot_api]
impl Signals {
    #[signal]
    fn play_click();
}
