using Godot;
using System;

using Timekiller.StateManager;

namespace Timekiller {
	public partial class User : GodotObject {
		private int gid;
		public (int Planet, int Region, int SubRegion, int Plot) Coords;

		public User((int, int, int, int) coords) {
			this.gid = Manager.GetNewGID();
			this.Coords = coords;
		}
	}
}
