using Godot;
using System;
using System.Linq;

namespace Timekiller.Terrain {
	public partial class SubRegion {
		public int ID;
		public Plot[] Plots;

		public SubRegion(int id) {
			this.ID = id;
			this.Plots = Enumerable.Range(0, 35).Select(id => new Plot(id)).ToArray();
		}

		public void Tick() {
			foreach (Plot plot in this.Plots) {
				plot.Tick();
			}
		}
	}
}
