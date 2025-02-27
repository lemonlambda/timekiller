use crate::terminal::Printer;

use anyhow::*;
use std::collections::HashMap;
use std::rc::Rc;
use std::sync::{Mutex, MutexGuard};

/// A struct to register commands with a printer
pub struct CommandManager<P: Printer> {
    commands: HashMap<String, fn(MutexGuard<'_, P>, Vec<String>)>,
    /// This might need to be shared between multiple structs so it needs to be safe
    printer: Rc<Mutex<P>>,
}

impl<P: Printer> CommandManager<P> {
    pub fn new(printer: Rc<Mutex<P>>) -> Self {
        Self {
            commands: HashMap::new(),
            printer,
        }
    }

    /// Registers a new command
    pub fn register_command<S: ToString>(
        &mut self,
        command_name: S,
        command: fn(MutexGuard<'_, P>, Vec<String>),
    ) {
        self.commands.insert(command_name.to_string(), command);
    }

    /// Processes a command
    /// Returns an error if the command doesn't exist
    pub fn process_command<S: ToString>(
        &mut self,
        command_name: S,
        args: Vec<String>,
    ) -> Result<()> {
        if let Some(command_func) = self.commands.get(&command_name.to_string()) {
            let locked_printer = self.printer.lock().unwrap();
            command_func(locked_printer, args);
        } else {
            return Err(anyhow!("Command {} is missing", command_name.to_string()));
        }

        Ok(())
    }
}
