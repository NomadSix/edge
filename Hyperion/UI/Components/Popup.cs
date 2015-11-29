using System;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion.UI.Components {
	public class Popup:Screen {
		public Int32 Width, Height;
		public Vector2 Location;

		public Popup(Game game, Vector2 location, Int32 width, Int32 height) : base(game) {
			Location = location;
			Width = width;
			Height = height;
		}

		public Popup(Game game, Rectangle location) : base(game) {
			Location = new Vector2(location.X, location.Y);
			Width = location.Width;
			Height = location.Height;
		}

		public void Kill() {
			that.Components.Remove(this);
		}
	}
}

