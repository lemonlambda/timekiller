#nullable enable
using Godot;
using System;
using System.Linq;

using Timekiller.Helpers;
using Timekiller.StateManager;

namespace Timekiller.Terrain {
	public partial class SolarSystem : GodotObject {
		public int GID;
		public string Name;
		public Lazy<Planet>[] Planets;

		public SolarSystem(string? name = null, int? planetCount = null) {
			this.GID = Manager.GetNewGID();
			this.Name = name ?? UniqueName.GenerateUniqueSystemName();
			Random r = new Random();
			int numberOfPlanets = planetCount ?? r.Next(1, 5);
			this.Planets = Enumerable.Range(0, numberOfPlanets).Select(
				id => new Lazy<Planet>( () => new Planet(id, $"{this.Name} {(char)('b' + id)}"))
			).ToArray();
			Manager.AddTrackedGIDObject(this.GID, this);
		}

		public Planet GetCoords(int planet) {
			return this.Planets[planet].Value;
		}

		public Region GetCoords(int planet, int region) {
			return this.Planets[planet].Value.Regions[region].Value;
		}

		public SubRegion GetCoords(int planet, int region, int subRegion) {
			return this.Planets[planet].Value.Regions[region].Value.SubRegions[subRegion].Value;
		}

		public Plot GetCoords(int planet, int region, int subRegion, int plot) {
			return this.Planets[planet].Value.Regions[region].Value.SubRegions[subRegion].Value.Plots[plot].Value;
		}

		public void Tick() {
			foreach (Lazy<Planet> planet in this.Planets) {
				if (planet.IsValueCreated) {
					planet.Value.Tick();
				}
			}
		}
	}
}
