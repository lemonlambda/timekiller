using Godot;
using System;

namespace Timekiller.Terrain {
	public partial class Region {
		public int id;
		public SubRegion[] subRegions;

		public Region(int id) {
			this.id = id;
			this.subRegions = new SubRegion[0];
		}
	}
}
