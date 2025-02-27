use std::rc::Rc;
use std::sync::Mutex;

/// A trait to inteface with something that has a stdin and stdout
pub trait Printer {
    /// Prints in the user type field
    fn stdin_print<T: ToString>(&mut self, content: T);
    /// Prints with newline in the user type field
    fn stdin_println<T: ToString>(&mut self, content: T);
    /// Prints in the command output field
    fn print<T: ToString>(&mut self, content: T);
    /// Prints with new line in the command output field
    fn println<T: ToString>(&mut self, content: T);
    /// Constructs the command history to a readable format
    fn flush(&mut self);
}

impl<P: Printer> Printer for Rc<Mutex<P>> {
    fn stdin_print<T: ToString>(&mut self, content: T) {
        let mut locked = self.lock().unwrap();
        locked.stdin_print(content);
    }
    fn stdin_println<T: ToString>(&mut self, content: T) {
        let mut locked = self.lock().unwrap();
        locked.stdin_println(content);
    }
    fn print<T: ToString>(&mut self, content: T) {
        let mut locked = self.lock().unwrap();
        locked.print(content);
    }
    fn println<T: ToString>(&mut self, content: T) {
        let mut locked = self.lock().unwrap();
        locked.println(content);
    }
    fn flush(&mut self) {
        let mut locked = self.lock().unwrap();
        locked.flush();
    }
}
