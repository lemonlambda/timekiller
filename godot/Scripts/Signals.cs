using Godot;
using System;

namespace Timekiller {
	public partial class Signals : Node {
		[Signal]
		public delegate void PlayClickEventHandler();
	}
}
