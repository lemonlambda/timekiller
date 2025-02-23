using Godot;
using System;

using Timekiller.StateManager;

namespace Timekiller {
	public partial class Terminal : RichTextLabel
	{
		public void PrintLn(string content) {
			this.Text += content + "\n";
		}
	
		public void Print(string content) {
			this.Text += content;
		}

		private void ProcessCommand(string commandName, string[] args) {
			switch (commandName) {
				case "":
					break;
				case "clear":
					this.Text = "";
					break;
				case "window":
					GetTree().Root.GetNode<Control>("Main/Window").Visible = true;
					break;
				case "examine":
					if (args.Length < 1) {
						this.PrintLn("No args provided, can't examine.");
						break;
					}

					switch (args[0]) {
						case "self":
							this.PrintLn("== You ==");
							this.PrintLn($"Coords: You don't exist");
							break;
						case "solarsystem":
							this.PrintLn("I'm gonna die");
							break;
						default:
							this.PrintLn($"You can't examine {args[0]}.");
							break;
					}

					break;
				case "exit": 
					// This quits the game
					GetTree().Quit();
					break;
				default:
					this.PrintLn($"Command {commandName} is unrecognized.");
					break;
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
