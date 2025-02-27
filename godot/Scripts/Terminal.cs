using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Timekiller.StateManager;
using Timekiller.Terrain;
using Timekiller.Helpers;
using Timekiller.Harvestables;

namespace Timekiller {
	public partial class Terminal : TerminalTextLabel {
		[Export]
		public bool EnableCutscene { get; set; } = true;
		private bool inCutscene = true;

		private int commandUp = 0;

		public async void PlayIntro() {
			if (!this.EnableCutscene) {
				this.inCutscene = false;
				return;
			}
		
			this.inCutscene = true;
			await ToSignal(GetTree().CreateTimer(2f), "timeout");

			await this.CutsceneType("Adam\n");
			this.Text += "Password: ";
			await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
			await this.CutsceneType("*******\n");
			this.Text += "\nWelcome Employee Adam.\n";
			this.Text += "Logging you in.";
			await ToSignal(GetTree().CreateTimer(1f), "timeout");
			this.Text += ".";
			await ToSignal(GetTree().CreateTimer(1f), "timeout");
			this.Text += ".";
			await ToSignal(GetTree().CreateTimer(1f), "timeout");
			this.inCutscene = false;
			this.Flush();
		}

		public async Task CutsceneType(string content) {
			Random random = new Random();
			foreach (char character in content) {
				this.Text += character.ToString();
				GD.Print($"{nameof(Signals.PlayClick)}");
				this.signals.EmitSignal(nameof(Signals.PlayClick));
				await ToSignal(GetTree().CreateTimer(.2 + (random.NextDouble() * 0.1 - 0.05)), "timeout");
			}
		}

		public override void _Ready() {
			this.commandHistory.Add(("", ""));
		
			this.PlayIntro();
		}

		private void ProcessCommand(string commandName, string[] args) {
			try {
				this.commandManager.ProcessCommand(commandName, args);
			} catch (CommandManagerError ex) {
				this.PrintLn(ex.Message);
			}
		}

		public override void _Input(InputEvent @event) {
			if (this.inCutscene) {
				return;
			}
		
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
