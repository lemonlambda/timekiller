using Godot;
using System;
using System.Linq;

namespace Timekiller {
	public partial class AudioPlayer : AudioStreamPlayer2D {
		private Signals signals;

		private AudioStream[] clicks = Enumerable.Range(0, 32).Select( id => GD.Load<AudioStream>($"res://Assets/Audio/keypress-{id:D2}.wav")).ToArray();
		private Random random = new Random();

		public override void _Ready() {
			this.signals = GetNode<Signals>("/root/Signals");
			this.signals.PlayClick += this.ClickSound;
		}

		private void ClickSound() {
			this.Stream = this.clicks[this.random.Next(0, clicks.Length-1)];
			this.Play();
			DateTime now = DateTime.Now; // Local time
	        GD.Print(now.ToString("C#: yyyy-MM-dd HH:mm:ss.fff")); 
		}
	}
}
