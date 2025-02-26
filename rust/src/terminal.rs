use godot::prelude::*;
use godot::classes::*;
use godot::global::*;

pub trait Printer {
    /// Prints in the user type field
    fn user_print<T: ToString>(&mut self, content: T); 
    /// Prints with newline in the user type field
    fn user_println<T: ToString>(&mut self, content: T); 
    /// Prints in the command output field
    fn print<T: ToString>(&mut self, content: T);
    /// Prints with new line in the command output field
    fn println<T: ToString>(&mut self, content: T); 
    /// Constructs the command history to a readable format
    fn flush(&mut self);
}

struct TerminalPrinter {
    /// Has a format of user type input and command output
    pub command_history: Vec<(String, String)>,

    /// Output for flush
    pub text: String
}

impl TerminalPrinter {
    fn new() -> Self {
        Self {
            command_history: vec![(String::new(), String::new())],
            text: String::new()
        }
    }
}

impl Printer for TerminalPrinter {
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
            format!("?> {}\n{}", command, output)
        }).fold(String::new(), |acc, history| format!("{}{}", acc, history));
        self.text = reconstructed;
    }
}


#[derive(GodotClass)]
#[class(base = RichTextLabel)]
struct Terminal {
    printer: TerminalPrinter,

    base: Base<RichTextLabel>
}

#[godot_api]
impl IRichTextLabel for Terminal {
    fn init(base: Base<RichTextLabel>) -> Self {
        Self {
            printer: TerminalPrinter::new(),
            base
        }
    }

    fn input(&mut self, event: Gd<InputEvent>) {
        if let Ok(input_event) = event.clone().try_cast::<InputEventKey>() && event.is_pressed() {
            match input_event.get_keycode() {
                Key::ENTER => {
                    self.printer.command_history.push((String::new(), String::new()));
                },
                Key::BACKSPACE => {
                    if self.printer.command_history.len() == 0 {
                        return;
                    }
                    let idx = self.printer.command_history.len() - 1;
                    self.printer.command_history[idx].0.pop();
                },
                _ => {
                    let character = char::from_u32(input_event.get_unicode() as u32).unwrap();
                    // prevents stupid shift and control keys from becoming real
                    if character.is_control() {
                        return;
                    }
                    
                    self.printer.user_print(character);
                }
            }
        }
        
        self.printer.flush();
        let text = self.printer.text.clone();
        self.base_mut().set_text(&text);
    }
}
