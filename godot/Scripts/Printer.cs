using Godot;
using System;

namespace Timekiller {
	public interface IPrinter {
		public void PrintLn(string content);
		public void Print(string content);
		public void Clear();
	}
}
