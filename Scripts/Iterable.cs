using Godot;
using System;

namespace Timekiller.Helpers {
	public partial class Iterable : GodotObject
	{
		private dynamic[] contents;

		public Iterable(dynamic[] array_contents) {
			contents = array_contents;
		}

		public Iterable Map(Func<dynamic, dynamic> func) {
			for (int i = 0; i < this.contents.Length; i++) {
				contents[i] = func(contents[i]);
			}

			return this;
		}

		public dynamic[] Collect() {
			return this.contents;
		}
	}
}
