using Godot;
using System;

using Timekiller.Helpers;
using Timekiller.StateManager;

namespace Timekiller.Terrain {
	public partial class SolarSystem : GodotObject {
		public int GID;
		public string Name;
		public Planet[] Planets;

		public SolarSystem() {
			this.GID = Manager.GetNewGID();
			this.Name = UniqueName.GenerateUniqueSystemName();
			this.Planets = new Planet[0];
		}
	}
}
