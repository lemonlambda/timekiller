using Godot;
using System;

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
			if (@event is InputEventKey keyEvent && keyEvent.Pressed) {
				string[] lines = this.Text.Split("\n");
				string current_line = lines[lines.Length-1];

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
						string[] args = new string[0];
						if (values.Length > 3) {
							args = values[2..];
						}

						this.ProcessCommand(commandName, args);
					
						this.Print("?> ");
						break;
					default:
						this.Print(((char)keyEvent.Unicode).ToString());
						break;
				}

				if (keyEvent.Keycode == Key.Backspace) {
				}
			
			}
		}
	
	}
}
