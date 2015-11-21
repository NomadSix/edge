using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Edge.Hyperion.UI.Components {
	public class Button:UIComponent {
		Boolean _hovering;
		Texture2D _base, _hover;
		Rectangle _location;
		ButtonAction _action;

		public delegate void ButtonAction ();

		public Button (Game game, Rectangle location, Texture2D texture, ButtonAction action) : base (game) {
			_location = location;
			_base = texture;
			_hover = texture;
			_action = action;
		}

		public Button (Game game, Rectangle location, Texture2D texture, Texture2D hover, ButtonAction action) : base (game) {
			_location = location;
			_base = texture;
			_hover = hover;
			_action = action;
		}

		public override void Update (GameTime gameTime) {
			_hovering = _location.Contains (that.mouse.Location);
			if (_hovering && that.mouse.IsButtonToggledUp (Edge.Hyperion.Input.MouseButtons.Left))
				_action ();
			base.Update (gameTime);
		}

		public override void Draw (GameTime gameTime) {
			that.spriteBatch.Draw (_hovering ? _hover : _base, _location, Color.White);
			base.Draw (gameTime);
		}
	}
}

