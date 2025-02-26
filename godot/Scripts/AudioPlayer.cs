using Godot;
using System;

namespace Timekiller {
	public partial class AudioPlayer : AudioStreamPlayer2D {
		private Signals signals;

		private AudioStream[] clicks = new AudioStream[] {
			GD.Load<AudioStream>("res://Assets/Audio/click1.wav"),
			GD.Load<AudioStream>("res://Assets/Audio/click2.wav"),
			GD.Load<AudioStream>("res://Assets/Audio/click3.wav"),
			GD.Load<AudioStream>("res://Assets/Audio/click4.wav"),
			GD.Load<AudioStream>("res://Assets/Audio/click5.wav"),
			GD.Load<AudioStream>("res://Assets/Audio/click6.wav"),
			GD.Load<AudioStream>("res://Assets/Audio/click7.wav"),
			GD.Load<AudioStream>("res://Assets/Audio/click8.wav"),
			GD.Load<AudioStream>("res://Assets/Audio/click9.wav"),
			GD.Load<AudioStream>("res://Assets/Audio/click10.wav"),
			GD.Load<AudioStream>("res://Assets/Audio/click11.wav"),
			GD.Load<AudioStream>("res://Assets/Audio/click12.wav"),
			GD.Load<AudioStream>("res://Assets/Audio/click13.wav"),
			GD.Load<AudioStream>("res://Assets/Audio/click14.wav"),
		};
		private Random random = new Random();

		public override void _Ready() {
			this.signals = GetNode<Signals>("/root/Signals");
			this.signals.PlayClick += this.ClickSound;
		}

		private void ClickSound() {
			this.Stream = this.clicks[this.random.Next(6, clicks.Length-1)];
			this.Play();
		}
	}
}
