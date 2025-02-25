using Godot;
using System;
using System.Collections.Generic;

using Timekiller.StateManager;

namespace Timekiller.Harvestables {
	public partial class Harvestable : GodotObject {
		private int gid;
		public string Name;
		public int Stage;
		public int MaxStage;
		public Dictionary<string, float> AmbientProduction; // What it makes ambiently, like oxygen
		public Dictionary<string, float> MatureResult; // What it makes when it's done growing like wood

		public Dictionary<string, float> Harvest() {
			if (this.Stage == this.MaxStage) {
				return this.MatureResult;
			}
			return new Dictionary<string, float>();
		}

		public void Tick() {
			this.Stage = Math.Min(this.MaxStage, this.Stage + 1);
		}

		public void Init(
			string name,
			int stage,
			int maxStage,
			Dictionary<string, float> ambientProduction,
			Dictionary<string, float> matureResult
		) {
			this.gid = Manager.GetNewGID();
			this.Name = name;
			this.MaxStage = maxStage;
			this.AmbientProduction = ambientProduction;
			this.MatureResult = matureResult;
		}

		public Harvestable() { }
	}
}
