use godot::classes::*;
use godot::global::*;
use godot::obj::NewAlloc;
use godot::prelude::*;
use rand::prelude::*;

#[derive(GodotClass)]
#[class(base = MarginContainer)]
pub struct Numbers {
    numbers: Vec<u8>,

    #[export]
    rows: f32,
    #[export]
    columns: f32,
    #[export]
    spacing: f32,

    #[export]
    button: Gd<CheckBox>,
    #[export]
    container: Gd<Panel>,

    random: ThreadRng,

    base: Base<MarginContainer>,
}

#[godot_api]
impl IMarginContainer for Numbers {
    fn init(base: Base<MarginContainer>) -> Self {
        Self {
            numbers: vec![],

            rows: 15.0,
            columns: 30.0,
            spacing: 3.0,
            button: CheckBox::new_alloc(),
            container: Panel::new_alloc(),
            random: rand::rng(),
            base,
        }
    }

    fn ready(&mut self) {
        let position = self.button.get_position();
        let (mut x, mut y) = (position.x, position.y);
        let size = self.button.get_size();
        let (dx, dy) = (size.x, size.y);

        for _ in 0..(self.rows as u32) {
            for _ in 0..(self.columns as u32) {
                let mut button = self.button.duplicate().unwrap().cast::<CheckBox>();
                let number = self.random.random_range(0..=9);
                self.numbers.push(number as u8);
                button.set_text(&number.to_string());
                button.set_position(Vector2::new(x, y));
                button.set_visible(true);
                self.container.add_child(&button);

                x += dx + self.spacing;
            }
            x = position.x;
            y += dy + self.spacing;
        }
    }
}
