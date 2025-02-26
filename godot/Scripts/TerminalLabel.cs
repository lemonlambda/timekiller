using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Timekiller {
	public partial class TerminalTextLabel : RichTextLabel, IPrinter {
		protected Signals signals;
		
		protected CommandManager commandManager;
		protected List<(string, string)> commandHistory = new List<(string, string)>();

		public override void _Ready() {
			this.commandManager = new TerminalCommandManager(this);
		
			// Load the signals
			this.signals = GetNode<Signals>("/root/Signals");
		}

		public void Type(string content) {
			int idx = this.commandHistory.Count - 1;
			(string, string) currentCommand = this.commandHistory[idx];

			currentCommand.Item1 += content;
			this.signals.EmitSignal(nameof(Signals.PlayClick));

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
				this.signals.EmitSignal(nameof(Signals.PlayClick));
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
	}
}
