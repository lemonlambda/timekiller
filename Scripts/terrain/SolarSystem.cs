using Godot;
using System;

using Timekiller.StateManager;

namespace Timekiller.Terrain {
	public partial class SolarSystem : GodotObject {
		public int GID;
		public Planet[] Planets;

		public SolarSystem() {
			this.GID = Manager.GetNewGID();
			this.Planets = new Planets[0];
		}
	}
}
