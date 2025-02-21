using Godot;
using System;

namespace Timekiller.Terrain {
	public partial class SubRegion {
		public int id;
		public Plot[] plots;

		public SubRegion(int id) {
			this.id = id;
			this.plots = new Plot[0];
		}
	}
}
