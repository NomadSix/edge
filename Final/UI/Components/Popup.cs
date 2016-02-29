using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Edge.Hyperion.UI.Components {
	public class Popup:Screen {
		public Int32 Width, Height;
        public Vector2 Location;
        public Texture2D backGround;

		public Popup(Game game, Vector2 location, Int32 width, Int32 height) : base(game) {
			Location = location;
			Width = width;
			Height = height;
		}

        public Popup(Game game, Vector2 location)
            : base(game) {
            Location = location;
        }

        protected override void LoadContent() {
            backGround = that.Content.Load<Texture2D>(@"../Images/Grey.png");
            base.LoadContent();
        }

		public void Kill() {
			that.Components.Remove(this);
		}
	}
}

