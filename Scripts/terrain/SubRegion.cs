using Godot;
using System;

namespace Timekiller.Terrain {
	public partial class SubRegion {
		public int ID;
		public Plot[] Plots;

		public SubRegion(int ID) {
			this.ID = id;
			this.Plots = new Plot[0];
		}
	}
}
