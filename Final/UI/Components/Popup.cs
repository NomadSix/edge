using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Edge.Hyperion.UI.Components {
	public class Popup:Screen {
        public Rectangle Location;
        public static Texture2D backGround;

        public Popup(Game game, int x, int y, int width, int height) : base(game) {
			Location = new Rectangle(x, y, width, height);
		}

        public Popup(Game game) : base(game) {
            Location = new Rectangle(0, 0, that.GraphicsDevice.Viewport.Width, that.GraphicsDevice.Viewport.Height);
        }

        public Popup(Game game, Rectangle location)
            : base(game) {
            Location = location;
        }

		public void Kill() {
			that.Components.Remove(this);
		}
	}
}

