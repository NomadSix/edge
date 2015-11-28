using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Edge.Hyperion.UI.Components {
	public class Button:UIComponent {
		Boolean _hovering;
		Texture2D _base;
		Rectangle _location;
        ButtonStyle _style;
		ButtonAction _action;

		public delegate void ButtonAction ();

		public Button (Game game, Rectangle location, ButtonStyle style, ButtonAction action) : base (game) {
			_location = location;
			_action = action;
            _base = style._Base;
            _style = style;
		}

		public override void Update (GameTime gameTime) {
			_hovering = _location.Contains (that.mouse.Location);
			if (_hovering && that.mouse.IsButtonToggledUp (Edge.Hyperion.Input.MouseButtons.Left))
				_action ();
			base.Update (gameTime);
		}

		public override void Draw (GameTime gameTime) {
            //for now we are going to just use a build in color
            //will be using a custom color later
			that.spriteBatch.Draw (_base, _location, _hovering ? Color.Gray : Color.White);
			base.Draw (gameTime);
		}
        public class ButtonStyle {
            public enum ButtonStyles : byte {
                test
            }
            public Texture2D _Base;
            public String _StyleName;
            public ButtonStyle(String StyleName, Texture2D Base) {
                _Base = Base;
                _StyleName = StyleName;
            }
        }
	}
}

