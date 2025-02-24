using Godot;
using System;
using System.Linq;

using Timekiller.Terrain;

namespace Timekiller.StateManager {
	public static class Manager {
		public static SolarSystem[] Systems = Enumerable.Range(0, 100).Select(id => id == 0 ? new SolarSystem("Zavka", (int?)3) : new SolarSystem()).ToArray();
	
		private static int gid = 0;

		public static int GetNewGID() {
			return gid++;
		}
	}
}
