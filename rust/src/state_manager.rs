use fragile::Fragile;
use lazy_static::lazy_static;
use std::{collections::HashMap, intrinsics::size_of, ptr::slice_from_raw_parts, sync::Mutex};

use crate::user::User;

static GIDC: Mutex<u64> = Mutex::new(0);
lazy_static! {
    pub static ref MANAGER: Mutex<StateManager> = Mutex::new(StateManager::new());
    static ref TETHERED_GIDS: Mutex<HashMap<u64, Fragile<*const [u8]>>> =
        Mutex::new(HashMap::new());
}

/// Global ID
pub struct Gid {
    value: u64,
}

impl Gid {
    pub fn new() -> Self {
        let mut locked = GIDC.lock().unwrap();
        let value = locked.clone();
        *locked += 1;
        Self { value }
    }

    /// Associates a GID with a certain item
    pub fn tether<T>(&self, item: &T) {
        let mut locked = TETHERED_GIDS.lock().unwrap();

        // Really fucky shit because I need to go from &T -> *const [u8]
        let ptr = item as *const T;
        let byte_ptr = ptr as *const u8;
        let size = size_of::<T>();

        locked.insert(
            self.value,
            Fragile::new(slice_from_raw_parts(byte_ptr, size)),
        );
    }

    /// Gets an item from a GID
    pub fn get_from_id<T>(&self) -> Option<&T> {
        let locked = TETHERED_GIDS.lock().unwrap();

        match locked.get(&self.value) {
            Some(value) => {
                // *const [u8] -> &T
                let inner = value.clone().into_inner();
                let ptr = inner as *const u8;

                Some(unsafe { &*(ptr as *const T) })
            }
            None => None,
        }
    }
}

/// Manages all the global state for the game
pub struct StateManager {
    pub player: User,
}

impl StateManager {
    pub fn new() -> Self {
        Self {
            player: User::new(),
        }
    }
}
