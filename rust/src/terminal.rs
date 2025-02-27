use crate::command_manager::CommandManager;
use crate::printer::Printer;
use crate::state_manager::MANAGER;

use godot::classes::*;
use godot::global::*;
use godot::prelude::*;
use std::rc::Rc;
use std::sync::Mutex;

struct TerminalPrinter {
    /// Has a format of user type input and command output
    pub command_history: Vec<(String, String)>,

    /// Output for flush
    pub text: String,
}

impl TerminalPrinter {
    fn new() -> Self {
        Self {
            command_history: vec![(String::new(), String::new())],
            text: String::new(),
        }
    }
}

impl Printer for TerminalPrinter {
    /// Prints in the user type field
    fn stdin_print<T: ToString>(&mut self, content: T) {
        if self.command_history.len() < 1 {
            panic!("Command History's length is 0");
        }

        let idx = self.command_history.len() - 1;
        self.command_history[idx].0 += &content.to_string();
    }

    /// Prints with newline in the user type field
    fn stdin_println<T: ToString>(&mut self, content: T) {
        self.stdin_print(content);
        self.stdin_print("\n");
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
        let reconstructed = self
            .command_history
            .iter()
            .map(|(command, output)| format!("?> {}\n{}", command, output))
            .fold(String::new(), |acc, history| format!("{}{}", acc, history));
        self.text = reconstructed;
    }
}

#[derive(GodotClass)]
#[class(base = RichTextLabel)]
struct Terminal {
    printer: Rc<Mutex<TerminalPrinter>>,
    command_manager: CommandManager<TerminalPrinter>,

    base: Base<RichTextLabel>,
}

#[godot_api]
impl IRichTextLabel for Terminal {
    fn init(base: Base<RichTextLabel>) -> Self {
        let printer = Rc::new(Mutex::new(TerminalPrinter::new()));
        Self {
            printer: printer.clone(),
            command_manager: CommandManager::new(printer.clone()),
            base,
        }
    }

    fn ready(&mut self) {
        self.command_manager
            .register_command("test", |mut printer, _| printer.println("Hello"));
        self.command_manager
            .register_command("coords", |mut printer, _| {
                let locked = MANAGER.lock().unwrap();
                godot_print!("Ran");
                printer.println(format!("{:?}", locked.player.position));
            });
    }

    fn input(&mut self, event: Gd<InputEvent>) {
        if let Ok(input_event) = event.clone().try_cast::<InputEventKey>()
            && event.is_pressed()
        {
            godot_warn!("Got keyboard event");
            match input_event.get_keycode() {
                Key::ENTER => {
                    godot_warn!("Got Enter");
                    let printer = self.printer.lock().unwrap();

                    // Get the command name and args seperately
                    let idx = printer.command_history.len() - 1;
                    let current_command_splitted = printer.command_history[idx]
                        .0
                        .split(" ")
                        .map(|s| s.to_string())
                        .collect::<Vec<String>>();
                    let current_command = current_command_splitted[0].clone();
                    let args = {
                        if current_command_splitted.len() > 1 {
                            current_command_splitted[1..].to_vec()
                        } else {
                            vec![]
                        }
                    };
                    drop(printer);

                    let command_result =
                        self.command_manager.process_command(current_command, args);

                    let mut printer = self.printer.lock().unwrap();
                    match command_result {
                        Ok(_) => {}
                        Err(error) => {
                            godot_warn!("Command error: {}", error);
                            printer.println(format!("{}", error));
                        }
                    }

                    printer.command_history.push((String::new(), String::new()));
                }
                Key::BACKSPACE => {
                    let mut printer = self.printer.lock().unwrap();
                    if printer.command_history.len() == 0 {
                        return;
                    }
                    let idx = printer.command_history.len() - 1;
                    printer.command_history[idx].0.pop();
                }
                _ => {
                    let character = char::from_u32(input_event.get_unicode() as u32).unwrap();
                    // prevents stupid shift and control keys from becoming real
                    if character.is_control() {
                        return;
                    }

                    self.printer.stdin_print(character);
                }
            }
        }

        self.printer.flush();
        let printer = self.printer.lock().unwrap();
        let text = printer.text.clone();
        drop(printer);
        self.base_mut().set_text(&text);
    }
}
