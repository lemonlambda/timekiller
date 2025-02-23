using Godot;
using System;
using System.Linq;

using Timekiller.StateManager;

namespace Timekiller {
	public partial class Terminal : RichTextLabel {
		private CommandManager commandManager = new CommandManager();
	
		public void PrintLn(string content) {
			this.Text += content + "\n";
		}
	
		public void Print(string content) {
			this.Text += content;
		}

		public override void _Ready() {
			this.commandManager.RegisterCommand("", null, (_) => { return; }, true);
			this.commandManager.RegisterCommand("clear", "Clears the screen of all text.", (_) => { this.Text = ""; }, false);
			this.commandManager.RegisterCommand("examine", "Examines a thing. Takes one arg.", (args) => {
				if (args.Length < 1) {
					this.PrintLn("No args provided, can't examine.");
					return;
				}

				switch (args[0]) {
					case "self":
						this.PrintLn("== You ==");
						this.PrintLn($"Coords: You don't exist");
						break;
					case "solarsystem":
						this.PrintLn($"System: {string.Join(", ", Manager.Systems[0].Planets.Select(planet => planet.Name))}");
						break;
					default:
						this.PrintLn($"You can't examine {args[0]}.");
						break;
				}
			}, false);
			this.commandManager.RegisterCommand("exit", "Exits the terminal.", (_) => { GetTree().Quit(); }, false);
			this.commandManager.RegisterCommand("help", "Gets help", (_) => {
				this.PrintLn(this.commandManager.GetHelp());
			}, true);
		}

		private void ProcessCommand(string commandName, string[] args) {
			try {
				this.commandManager.ProcessCommand(commandName, args);
			} catch (CommandManagerError ex) {
				this.PrintLn(ex.Message);
			}
		}
	
		public override void _Input(InputEvent @event) {
			string[] lines = this.Text.Split("\n");
			if (lines.Length > 22) {
				this.Text = string.Join("\n", lines[1..]);
			}
			
			string current_line = lines[lines.Length-1];
			if (@event is InputEventKey keyEvent && keyEvent.Pressed) {

				switch (keyEvent.Keycode) {
					case Key.Backspace:
						if (current_line.Length > 3) {
							this.Text = this.Text.Remove(this.Text.Length - 1);
						}
						break;
					case Key.Enter:
						this.Print("\n");
						string[] values = current_line.Split(" ");
						string commandName = values[1];
						string[] args = new string[values.Length-2];
						if (values.Length > 2) {
							args = values[2..];
						}

						this.ProcessCommand(commandName, args);
					
						this.Print("?> ");
						break;
					default:
						this.Print(((char)keyEvent.Unicode).ToString());
						break;
				}
			}
		}
	
	}
}
