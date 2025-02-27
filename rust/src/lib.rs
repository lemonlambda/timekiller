#![feature(let_chains)]

mod command_manager;
mod printer;
mod terminal;
mod terrain;

use godot::prelude::*;

struct TimekillerExtension;

#[gdextension]
unsafe impl ExtensionLibrary for TimekillerExtension {}
