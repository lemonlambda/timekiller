#![allow(internal_features)]
#![feature(let_chains, const_trait_impl, core_intrinsics)]

mod case;
mod click_audio;
mod command_manager;
mod encrypted_file;
mod helpers;
mod numbers;
mod printer;
mod signals;
mod state_manager;
mod terminal;
mod user;

use godot::prelude::*;

struct TimekillerExtension;

#[gdextension]
unsafe impl ExtensionLibrary for TimekillerExtension {}
