using Godot;
using System;

namespace Timekiller {
	public partial class CloseWindow : Button {
		public override void _Pressed() {
			GetTree().Root.GetNode<Control>("Main/Window").Visible = false;
		}
	}
}
