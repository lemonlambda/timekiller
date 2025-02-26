using Godot;
using System;
using System.Linq;

namespace Timekiller.Terrain {
	public partial class SubRegion {
		public int ID;
		public Lazy<Plot>[] Plots;

		public SubRegion(int id) {
			this.ID = id;
			this.Plots = Enumerable.Range(0, 35).Select(
				id => new Lazy<Plot>(() => new Plot(id))
			).ToArray();
		}

		public void Tick() {
			foreach (Lazy<Plot> plot in this.Plots) {
				if (plot.IsValueCreated) {
					plot.Value.Tick();
				}
			}
		}
	}
}
