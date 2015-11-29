using System;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion.UI.Components {
	public class UIComponent:DrawableGameComponent {
		protected Hyperion that;
        protected Camera2D cam;

		public UIComponent(Game game) : base(game) {
			that = (Hyperion)game;
            cam = new Camera2D(Vector2.Zero);
		}
	}
}

