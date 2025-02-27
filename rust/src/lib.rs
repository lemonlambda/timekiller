#![allow(internal_features)]
#![feature(let_chains, const_trait_impl, core_intrinsics)]

mod command_manager;
mod helpers;
mod printer;
mod state_manager;
mod terminal;
mod terrain;
mod user;

use godot::prelude::*;

struct TimekillerExtension;

#[gdextension]
unsafe impl ExtensionLibrary for TimekillerExtension {}
