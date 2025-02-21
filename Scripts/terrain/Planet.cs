using Godot;
using System;

using Timekiller.StateManager;

namespace Timekiller.Terrain {
	public partial class Planet : GodotObject {
		public int gid;
		public string name;
		public Region[] regions;

		public Planet(string planet_name) {
			this.gid = Manager.GetNewGID();
			this.name = planet_name;
			this.regions = new Region[0];
		}
	}
}
