using Godot;
using System;
using System.Linq;
using System.Diagnostics;

using Timekiller.Helpers;
using Timekiller.Terrain;
using Timekiller.StateManager;

namespace Timekiller {
	public partial class TerminalCommandManager : CommandManager {
		public IPrinter printer;

		public TerminalCommandManager(IPrinter printer) : base() {
			this.printer = printer;
		
			this.RegisterCommand("", null, (_) => { return; }, true);
			this.RegisterCommand("clear", "Clears the screen of all text.", (_) => { 
				this.printer.Clear();
			}, false);
			this.RegisterCommand("examine", "Examines a thing. Takes one arg.", (args) => {
				if (args.Length < 1) {
					this.printer.PrintLn("No args provided, can't examine.");
					return;
				}

				switch (args[0]) {
					case "position":
						(int system, int planet, int region, int subRegion, int plot) coords = Manager.Player.Coords;
						var plot = ((SolarSystem)Manager.TrackedGIDObjects[coords.system]).GetCoords(coords.planet, coords.region, coords.subRegion, coords.plot);
					
						this.printer.PrintLn("== You ==");
						this.printer.PrintLn($"Coords: ({coords.system}, {coords.planet}, {coords.region}, {coords.subRegion}, {coords.plot})");
						this.printer.PrintLn($"System: {string.Join(", ", Manager.Systems[coords.system].Planets.Select(planet => planet.Value.Name))}");
						this.printer.PrintLn($"Plot:");
						this.printer.PrintLn($"\tHarvestable Count: {plot.Harvestables.Count}");
						break;
					case "harvestables":
						coords = Manager.Player.Coords;
						plot = ((SolarSystem)Manager.TrackedGIDObjects[coords.system]).GetCoords(coords.planet, coords.region, coords.subRegion, coords.plot);

						this.printer.PrintLn("== Plot ==");
						this.printer.PrintLn($"{string.Join("\n", plot.Harvestables[..10].Zip(Enumerable.Range(0, 10), (h, id) => $"{id} - {h.Name} ({h.Stage} / {h.MaxStage})").ToArray())}\n...");
						break;
					default:
						this.printer.PrintLn($"You can't examine {args[0]}.");
						break;
				}
			}, false);
			this.RegisterCommand("harvest", "Harvest a harvestable. Takes one arg.", (args) => {
				int harvestableNumber;
				try {
					harvestableNumber = Sanitizer.Sanitize(args, 0);
				} catch (SanitizerError er) {
					this.printer.PrintLn(er.Message);
					return;
				}
			
				(int system, int planet, int region, int subRegion, int plot) coords = Manager.Player.Coords;
				var plot = ((SolarSystem)Manager.TrackedGIDObjects[coords.system]).GetCoords(coords.planet, coords.region, coords.subRegion, coords.plot);
				if (harvestableNumber > plot.Harvestables.Count) {
					this.printer.PrintLn("That harvestable doesn't exist on this plot.");
					return;
				}
				var result = plot.Harvestables[harvestableNumber].Harvest();
				this.printer.PrintLn("Harvested.");
			}, false);
			this.RegisterCommand("exit", "Exits the terminal.", (_) => { GetTree().Quit(); }, false);
			this.RegisterCommand("man", "Gets help", (_) => {
				this.printer.PrintLn(this.GetHelp());
			}, true);
			this.RegisterCommand("tick", "Moves forward one tick.", (_) => {
				Stopwatch stopWatch = new Stopwatch();
				stopWatch.Start();
				Manager.Tick();
				stopWatch.Stop();
				this.printer.PrintLn($"Tick took {stopWatch.Elapsed.TotalSeconds} seconds");
			}, false);
		}
	}
}
