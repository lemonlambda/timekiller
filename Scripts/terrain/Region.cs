using Godot;
using System;

namespace Timekiller.Terrain {
	public partial class Region {
		public int ID;
		public SubRegion[] SubRegions;

		public Region(int id) {
			this.ID = id;
			this.SubRegions = new SubRegion[0];
		}

		public void Tick() {
			foreach (SubRegion subRegion in this.SubRegions) {
				subRegion.Tick();
			}
		}
	}
}
