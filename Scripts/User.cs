using Godot;
using System;

using Timekiller.StateManager;

namespace Timekiller {
	public partial class User : GodotObject {
		private int gid;
		public (int System, int Planet, int Region, int SubRegion, int Plot) Coords;

		public User(int system, int planet, int region, int subRegion, int plot) {
			this.gid = Manager.GetNewGID();
			this.Coords = (system, planet, region, subRegion, plot);
		}
	}
}
