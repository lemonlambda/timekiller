using Godot;
using System;

namespace Timekiller.Helpers {
	public class SanitizerError : Exception {
		public SanitizerError(string message) : base(message) {}
	}

	public static class Sanitizer {
		public static int Sanitize(string[] args, int index) {
			if (args.Length - 1 < index) {
				throw new SanitizerError($"No argument provided at position {index}.");
			}
			
			if (Int32.TryParse(args[0], out int parsedNumber)) {
				return parsedNumber;
			} else {
				throw new SanitizerError($"Argument at position {index} is not an integer.");
			}
		}
	}
}
