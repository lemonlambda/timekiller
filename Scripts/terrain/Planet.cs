using Godot;
using System;

namespace Timekiller.Terrain {
	public partial class Planet : GodotObject {
		public int ID;
		public string Name;
		public Region[] Regions;

		public Planet(int id, string planetName) {
			this.ID = id;
			this.Name = planetName;
			this.Regions = new Region[0];
		}
	}
}
