using Godot;
using System;

namespace Timekiller.StateManager {
	public partial class Manager : Node {
		public static Manager Instance { get; private set; }
	
		private static int gid = 0;

		public static int GetNewGID() {
			return gid++;
		}

		public override void _Ready() {
			Instance = this;
		}
	}
}
