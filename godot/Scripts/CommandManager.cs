#nullable enable
using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Timekiller {
	public class CommandManagerError : Exception {
		public CommandManagerError(string message) : base(message) {}
	}

	public partial class CommandManager : Node {
		private Dictionary<string, Action<string[]>> commands;
		private Dictionary<string, string> commandDescriptions;
		private int lastLength;
		private string cachedHelp;

		public CommandManager() {
			this.commands = new Dictionary<string, Action<string[]>>();
			this.commandDescriptions = new Dictionary<string, string>();
			this.lastLength = 0;
			this.cachedHelp = "";
		}

		public void RegisterCommand(string name, string? description, Action<string[]>? function, bool hidden) {
			if (!(function is null)) {
				this.commands.Add(name, function);
			}
			if (!hidden && !(description is null)) {
				this.commandDescriptions.Add(name, description!);
			}
		}

		public void ProcessCommand(string name, string[] args) {
			if (this.commands.ContainsKey(name)) {
				this.commands[name](args);
			} else {
				throw new CommandManagerError($"Command `{name}` does not exist.");
			}
		}

		public string GetHelp() {
			if (lastLength == this.commandDescriptions.Count) {
				return this.cachedHelp;
			}
			this.lastLength = this.commandDescriptions.Count;
			this.cachedHelp = "== Commands ==\n" + string.Join("\n", this.commandDescriptions.Select(kvp => $"{kvp.Key} - {kvp.Value}"));
			return this.cachedHelp;
		}
	}
}
