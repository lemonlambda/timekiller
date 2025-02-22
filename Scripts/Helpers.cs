using Godot;
using System;
using System.Collections.Generic;

namespace Timekiller.Helpers {
	public static class UniqueName {
		private static HashSet<string> usedNames = new HashSet<string>();
		private static string[] names = new string[100]{"Aether", "Vesper", "Nova", "Orion", "Altara", "Zenith", "Draco", "Lyra", "Seraph", "Solis", "Vega", "Lyrae", "Nexus", "Obsidian", "Andromeda", "Cassiopeia", "Orionis", "Phoenix", "Aquila", "Astra", "Zenithar", "Pyris", "Nebula", "Vulcan", "Celestia", "Helios", "Hydra", "Luna", "Eclipse", "Vortex", "Astrae", "Titan", "Quasar", "Exo", "Astraeus", "Helios", "Titanus", "Regalis", "Equinox", "Nebulon", "Solaris", "Hypnos", "Quanta", "Cosmos", "Scorpius", "Equinox", "Cygnet", "Polaris", "Andara", "Draconis", "Regalus", "Stellaris", "Phoenixus", "Auralis", "Velaris", "Caelus", "Spectra", "Chronos", "Galaxion", "Astraon", "Aethon", "Triton", "Zephyr", "Alpha", "Neptus", "Titania", "Borealis", "Empyrean", "Andros", "Thalasson", "Lyricon", "Solis", "Pyron", "Elysium", "Nexon", "Arcturus", "Stratos", "Aquarion", "Umbra", "Syphera", "Seraphis", "Helion", "Ignis", "Apollon", "Auron", "Stellon", "Calypso", "Lunaris", "Galaxar", "Corae", "Obsidian", "Epsilon", "Eldora", "Meteoris", "Astrolis", "Syntaris", "Venora", "Radion", "Altura", "Pyros"};
		
		public static string GenerateUniqueSystemName() {
			Random r = new Random();
		
			while (true) {
				int idx = r.Next(0, names.Length-1);
				string name = names[idx];
				int number = r.Next(0, 99);
				string systemName = $"{name}-{number.ToString()}";
				
				if (!usedNames.Contains(systemName)) {
					usedNames.Add(systemName);
					return systemName;
				}
			}
		}
	}
}
