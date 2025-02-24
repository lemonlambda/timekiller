using Godot;
using System;

namespace Timekiller.Harvestables {
	public partial class Tree : Harvestable {
		public Tree() : base() {
			Dictionary<string, float> ambient = new Dictionary();
			Dictionary<string, float> mature = new Dictionary();
			mature.add("wood", 2.0)
			
			this.init("tree", 0, 5, ambient, mature);
		}
	}
}
