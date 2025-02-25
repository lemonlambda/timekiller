using Godot;
using System;
using System.Linq;

namespace Timekiller.Terrain {
	public partial class Region {
		public int ID;
		public SubRegion[] SubRegions;

		public Region(int id) {
			this.ID = id;
			this.SubRegions = Enumerable.Range(0, 8).Select(id => new SubRegion(id)).ToArray();
		}

		public void Tick() {
			foreach (SubRegion subRegion in this.SubRegions) {
				subRegion.Tick();
			}
		}
	}
}
