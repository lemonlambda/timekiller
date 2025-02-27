use std::ops::{Deref, DerefMut};

pub struct Initable<T> {
    value: Option<T>,
}

impl<T> Initable<T> {
    pub fn new() -> Self {
        Self { value: None }
    }

    pub fn is_init(&self) -> bool {
        self.value.is_some()
    }

    pub fn init(&mut self, value: T) {
        self.value = Some(value);
    }
}

impl<T> Deref for Initable<T> {
    type Target = T;

    fn deref(&self) -> &Self::Target {
        match self.value.as_ref() {
            Some(value) => value,
            None => panic!("Initable is not initiated."),
        }
    }
}
impl<T> DerefMut for Initable<T> {
    fn deref_mut(&mut self) -> &mut Self::Target {
        match self.value.as_mut() {
            Some(value) => value,
            None => panic!("Initable is not initiated."),
        }
    }
}
