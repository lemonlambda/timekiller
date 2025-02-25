using Godot;
using System;

namespace Timekiller.Terrain {
	public partial class SubRegion {
		public int ID;
		public Plot[] Plots;

		public SubRegion(int id) {
			this.ID = id;
			this.Plots = new Plot[0];
		}

		public void Tick() {
			foreach (Plot plot in this.Plots) {
				plot.Tick();
			}
		}
	}
}
