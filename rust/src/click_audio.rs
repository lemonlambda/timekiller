use godot::classes::*;
use godot::global::*;
use godot::prelude::*;
use rand::prelude::*;

use crate::signals::Signals;

#[derive(GodotClass)]
#[class(base = AudioStreamPlayer2D)]
pub struct ClickPlayer {
    random: ThreadRng,
    clicks: Vec<Gd<AudioStream>>,

    base: Base<AudioStreamPlayer2D>,
}

#[godot_api]
impl ClickPlayer {
    #[func]
    fn play_click(&mut self) {
        let sound = self.clicks.clone();
        let sound = sound.choose(&mut self.random);

        self.base_mut().set_stream(sound);
        self.base_mut().play();
    }
}

#[godot_api]
impl IAudioStreamPlayer2D for ClickPlayer {
    fn init(base: Base<AudioStreamPlayer2D>) -> Self {
        Self {
            random: rand::rng(),
            clicks: (1..32)
                .into_iter()
                .map(|id| {
                    load::<AudioStream>(
                        format!("res://Assets/Audio/keypress-{:#02}.wav", id).as_str(),
                    )
                })
                .collect::<Vec<Gd<AudioStream>>>(),

            base,
        }
    }

    fn ready(&mut self) {
        let mut signals = self.base_mut().get_node_as::<Signals>("/root/Main/Signals");
        signals.connect("play_click", &self.base_mut().callable("play_click"));
    }
}
