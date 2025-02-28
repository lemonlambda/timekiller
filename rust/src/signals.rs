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
    // Signals for terminal
    #[signal]
    fn play_click();

    // Signals for numbers
    #[signal]
    fn number_pressed(id: i64, value: u8);
}
