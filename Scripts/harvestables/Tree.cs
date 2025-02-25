using Godot;
using System;
using System.Collections.Generic;

namespace Timekiller.Harvestables {
	public partial class Tree : Harvestable {
		public Tree() : base() {
			Dictionary<string, float> ambient = new Dictionary<string, float>();
			Dictionary<string, float> mature = new Dictionary<string, float>();
			mature.Add("wood", 2f);
			
			this.Init("tree", 0, 5, ambient, mature);
		}
	}
}
