using Godot;
using System;
using System.Linq;

using Timekiller.Helpers;
using Timekiller.StateManager;

namespace Timekiller.Terrain {
	public partial class SolarSystem : GodotObject {
		public int GID;
		public string Name;
		public Planet[] Planets;

		public SolarSystem() {
			this.GID = Manager.GetNewGID();
			this.Name = UniqueName.GenerateUniqueSystemName();
			Random r = new Random();
			int numberOfPlanets = r.Next(0, 5);
			this.Planets = Enumerable.Range(0, numberOfPlanets).Select(id => new Planet(id, $"{this.Name}{(char)('a' + id)}")).ToArray();
		}
	}
}
