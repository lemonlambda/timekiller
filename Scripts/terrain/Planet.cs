using Godot;
using System;
using System.Linq;

namespace Timekiller.Terrain {
	public partial class Planet : GodotObject {
		public int ID;
		public string Name;
		public Region[] Regions;

		public Planet(int id, string planetName) {
			this.ID = id;
			this.Name = planetName;
			this.Regions = Enumerable.Range(0, 10).Select(id => new Region(id)).ToArray();
		}

		public void Tick() {
			foreach (Region region in this.Regions) {
				region.Tick();
			}
		}
	}
}
