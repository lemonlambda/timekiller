using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

using Timekiller.StateManager;
using Timekiller.Terrain;
using Timekiller.Harvestables;

namespace Timekiller {
	public partial class Terminal : RichTextLabel {
		private CommandManager commandManager = new CommandManager();
		private List<(string, string)> commandHistory = new List<(string, string)>();
		private int commandUp = 0;

		public void Type(string content) {
			int idx = this.commandHistory.Count - 1;
			(string, string) currentCommand = this.commandHistory[idx];

			currentCommand.Item1 += content;

			this.commandHistory[idx] = currentCommand;
		}

		public void ReplaceType(string content) {
			int idx = this.commandHistory.Count - 1;
			(string, string) currentCommand = this.commandHistory[idx];

			currentCommand.Item1 = content;

			this.commandHistory[idx] = currentCommand;
		}

		public void Backspace() {
			int idx = this.commandHistory.Count - 1;
			(string, string) currentCommand = this.commandHistory[idx];
		
			if (currentCommand.Item1.Length > 0) {
				currentCommand.Item1 = currentCommand.Item1.Remove(currentCommand.Item1.Length - 1);
			}

			this.commandHistory[idx] = currentCommand;
		}
	
		public void Print(string content) {
			int idx = this.commandHistory.Count - 1;
			(string, string) currentCommand = this.commandHistory[idx];

			currentCommand.Item2 += content;

			this.commandHistory[idx] = currentCommand;
		}

		public void PrintLn(string content) {
			this.Print(content);
			this.Print("\n");
		}
	
		public new void Clear() {
			this.commandHistory = new List<(string, string)>();
			this.commandHistory.Add(("", ""));
		}

		// Construct the Text out of commandHistory
		public void Flush() {
			Func<(string, string), string> newline = command => (command.Item2 == "") ? "\n" : "";
			this.Text = string.Join("", this.commandHistory.Select(command => $"?> {command.Item1}\n{command.Item2}"));
		}

		public override void _Ready() {
			this.commandManager.RegisterCommand("", null, (_) => { return; }, true);
			this.commandManager.RegisterCommand("clear", "Clears the screen of all text.", (_) => { 
				this.Clear();
			}, false);
			this.commandManager.RegisterCommand("examine", "Examines a thing. Takes one arg.", (args) => {
				if (args.Length < 1) {
					this.PrintLn("No args provided, can't examine.");
					return;
				}

				switch (args[0]) {
					case "position":
						(int system, int planet, int region, int subRegion, int plot) coords = Manager.Player.Coords;
						var plot = ((SolarSystem)Manager.TrackedGIDObjects[coords.system]).GetCoords(coords.planet, coords.region, coords.subRegion, coords.plot);
					
						this.PrintLn("== You ==");
						this.PrintLn($"Coords: ({coords.system}, {coords.planet}, {coords.region}, {coords.subRegion}, {coords.plot})");
						this.PrintLn($"System: {string.Join(", ", Manager.Systems[coords.system].Planets.Select(planet => planet.Value.Name))}");
						this.PrintLn($"Plot:");
						this.PrintLn($"\tHarvestable Count: {plot.Harvestables.Count}");
						break;
					default:
						this.PrintLn($"You can't examine {args[0]}.");
						break;
				}
			}, false);
			this.commandManager.RegisterCommand("exit", "Exits the terminal.", (_) => { GetTree().Quit(); }, false);
			this.commandManager.RegisterCommand("man", "Gets help", (_) => {
				this.PrintLn(this.commandManager.GetHelp());
			}, true);
			this.commandManager.RegisterCommand("tick", "Moves forward one tick.", (_) => {
				Manager.Tick();
			}, false);
			this.commandHistory.Add(("", ""));
		}

		private void ProcessCommand(string commandName, string[] args) {
			try {
				this.commandManager.ProcessCommand(commandName, args);
			} catch (CommandManagerError ex) {
				this.PrintLn(ex.Message);
			}
		}

		public override void _Input(InputEvent @event) {
			if (@event is InputEventKey keyEvent && keyEvent.Pressed) {
				switch (keyEvent.Keycode) {
					case Key.Backspace:
						this.commandUp = 0;

						this.Backspace();
						break;
					case Key.Enter:
						int idx = this.commandHistory.Count - 1;
						string currentCommand = this.commandHistory[idx].Item1;
						string[] splitted = currentCommand.Split(" ");

						try {
							this.commandManager.ProcessCommand(splitted[0], splitted.Length > 1 ? splitted[1..] : new string[0] {});
						} catch (CommandManagerError er) {
							this.PrintLn(er.Message);
						}
						
						this.commandHistory.Add(("", ""));
					
						break;
					case Key.Up:
						this.commandUp = Math.Min(this.commandUp + 1, this.commandHistory.Count - 1);
					
						idx = this.commandHistory.Count - 1 - this.commandUp;
						string command = this.commandHistory[idx].Item1;

						this.ReplaceType(command);

						break;
					case Key.Down:
						this.commandUp = Math.Max(1, this.commandUp - 1);
					
						idx = this.commandHistory.Count - 1 - this.commandUp;
						command = this.commandHistory[idx].Item1;

						this.ReplaceType(command);
						break;
					default:
						this.commandUp = 0;

						char text = (char)keyEvent.Unicode;
						if (!char.IsControl(text)) {
							this.Type(text.ToString());
						}

						break;
				}
			}

			this.Flush();
		}
	}
}
