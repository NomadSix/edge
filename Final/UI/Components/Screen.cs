using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Edge.Hyperion.UI.Components {
	public class Screen:UIComponent {
		//TODO: Background texture?
		//TODO: How to handle popup finishing?
		Popup _activePopup;
        public Boolean _isActive = true;
        public readonly Viewport viewport;
        protected Camera2D cam;

		public Screen(Game game) : base(game) {
            cam = new Camera2D(Vector2.Zero, that.GraphicsDevice);
            viewport = that.GraphicsDevice.Viewport;
		}

		public override void Update(GameTime gameTime) {
            that.viewMatrix = cam.ViewMatrix;
			base.Update(gameTime);
		}

		protected void SetPopup(Popup popup) {
			if(_activePopup == null)
				_activePopup = popup;
		}
	}
}

