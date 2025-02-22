using Godot;
using System;

using Timekiller.Terrain;

namespace Timekiller.StateManager {
	public static class Manager {
		public static SolarSystem[] Systems = new SolarSystem[1]{new SolarSystem()};
	
		private static int gid = 0;

		public static int GetNewGID() {
			return gid++;
		}
	}
}
