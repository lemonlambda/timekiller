use crate::state_manager::Gid;

pub struct User {
    pub gid: Gid,
    pub position: (u32, u32, u32, u32, u32),
}

impl User {
    pub fn new() -> Self {
        let this = Self {
            gid: Gid::new(),
            position: (0, 0, 0, 0, 0),
        };

        this.gid.tether(&this);
        this
    }
}
