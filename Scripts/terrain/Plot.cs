using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

using Timekiller.Harvestables;

namespace Timekiller.Terrain {
	public partial class Plot {
		public int ID;
		public List<Harvestable> Harvestables;

		public Plot(int id) {
			this.ID = id;
			this.Harvestables = Enumerable.Range(0, 100).Select(_ => new Harvestables.Tree()).Cast<Harvestable>().ToList();
		}

		public void Tick() {
			foreach (Harvestable harvestable in this.Harvestables) {
				harvestable.Tick();
			}
		}
	}
}
