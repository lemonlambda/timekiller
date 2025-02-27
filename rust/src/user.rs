use crate::state_manager::Gid;

pub struct User {
    pub gid: Gid,
}

impl User {
    pub fn new() -> Self {
        let this = Self { gid: Gid::new() };

        this.gid.tether(&this);
        this
    }
}
