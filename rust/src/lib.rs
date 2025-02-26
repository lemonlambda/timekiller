#![feature(let_chains)]

mod terminal;
mod command_manager;

use godot::prelude::*;

struct TimekillerExtension;

#[gdextension]
unsafe impl ExtensionLibrary for TimekillerExtension {}
