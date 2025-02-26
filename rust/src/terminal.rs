use godot::prelude::*;
use godot::classes::*;
use godot::global::*;

#[derive(GodotClass)]
#[class(base = RichTextLabel)]
struct Terminal {
    /// Has a format of user type input and command output
    command_history: Vec<(String, String)>,

    base: Base<RichTextLabel>
}

#[godot_api]
impl Terminal {
    /// Prints in the user type field
    fn user_print<T: ToString>(&mut self, content: T) {
        if self.command_history.len() < 1 {
            panic!("Command History's length is 0");
        }
        
        let idx = self.command_history.len() - 1;
        self.command_history[idx].0 += &content.to_string();
    } 
    
    /// Prints with newline in the user type field
    fn user_println<T: ToString>(&mut self, content: T) {
        self.user_print(content);
        self.user_print("\n");
    } 

    /// Prints in the command output field
    fn print<T: ToString>(&mut self, content: T) {
        if self.command_history.len() < 1 {
            panic!("Command History's length is 0");
        }
        
        let idx = self.command_history.len() - 1;
        self.command_history[idx].1 += &content.to_string();
    }

    /// Prints with new line in the command output field
    fn println<T: ToString>(&mut self, content: T) {
        self.print(content);
        self.print("\n");
    }
    
    /// Constructs the command history to a readable format
    fn flush(&mut self) {
        let reconstructed = self.command_history.iter().map(|(command, output)| {
            format!("?>{}\n{}", command, output)
        }).fold(String::new(), |acc, history| format!("{}{}", acc, history));
        self.base_mut().set_text(&reconstructed);
    }
}

#[godot_api]
impl IRichTextLabel for Terminal {
    fn init(base: Base<RichTextLabel>) -> Self {
        Self {
            command_history: vec![(String::new(), String::new())],
            base
        }
    }

    fn input(&mut self, event: Gd<InputEvent>) {
        if let Ok(input_event) = event.clone().try_cast::<InputEventKey>() && event.is_pressed() {
            match input_event.get_keycode() {
                Key::BACKSPACE => {
                    if self.command_history.len() == 0 {
                        return;
                    }
                    let idx = self.command_history.len() - 1;
                    self.command_history[idx].0.pop();
                },
                _ => {
                    self.user_print(char::from_u32(input_event.get_unicode() as u32).unwrap());
                }
            }
        }
        
        self.flush();
    }
}
