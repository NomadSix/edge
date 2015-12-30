using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mouse = Edge.Hyperion.Input.Mouse;

namespace Edge.Hyperion.UI.Components {
	public class Button:UIComponent {
		Boolean _hovering;
        Screen _screen;
        Vector2 _textLocation;
		internal Rectangle _location;
		readonly ButtonStyle _style;
		readonly ButtonAction _action;
		readonly String _text;
        readonly Vector2 _measurements;

		public delegate void ButtonAction();

		public Button(Game game, Screen screen, Rectangle location, ButtonStyle style, String text, ButtonAction action) : base(game) {
			_location = location;
			_action = action;
			_style = style;
			_text = text;
            _screen = screen;
            _measurements = _style.Font.MeasureString(_text);
		}

		public override void Update(GameTime gameTime) {
			_hovering = _location.Contains(that.mouse.Location);
			if(_hovering && that.mouse.IsButtonToggledUp(Mouse.MouseButtons.Left) && _screen._isActive)
                _action();
            // I know this is Bad but i dont want to think of how to make it better just took act
            _textLocation = new Vector2(_location.Width / 2f - _measurements.X / 2f + _location.X, _location.Height / 2f - _measurements.Y / 2f + _location.Y);
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {
			that.batch.Draw(_hovering ? _style.Hover : _style.Base, _location, _hovering && _screen._isActive ? _style.HoverColour : _style.BaseColour);
			that.batch.DrawString(_style.Font, _text, _textLocation, _style.TextColour);
			base.Draw(gameTime);
		}

		public class ButtonStyle {
			public enum ButtonStyles : byte {
				basic
			}

			public Texture2D Base, Hover;
			public Color BaseColour, HoverColour, TextColour;
			public SpriteFont Font;

			public ButtonStyle(Texture2D texture, Texture2D hover, Color? baseColour, Color? hoverColour, Color? textColour) {
				Base = texture;
				Hover = hover;
				BaseColour = baseColour ?? Color.LightGray;
				HoverColour = hoverColour ?? Color.White;
				TextColour = textColour ?? Color.Black;
				Font = Edge.Hyperion.Backing.AssetStore.FontMain;
			}

			public ButtonStyle(Texture2D texture, Color? baseColour, Color? hoverColour, Color? textColour) {
				Base = texture;
				Hover = texture;
				BaseColour = baseColour ?? Color.LightGray;
				HoverColour = hoverColour ?? Color.White;
				TextColour = textColour ?? Color.Black;
				Font = Edge.Hyperion.Backing.AssetStore.FontMain;
			}

			public ButtonStyle(Texture2D texture, Texture2D hover, SpriteFont font, Color? baseColour, Color? hoverColour, Color? textColour) {
				Base = texture;
				Hover = hover;
				BaseColour = baseColour ?? Color.LightGray;
				HoverColour = hoverColour ?? Color.White;
				TextColour = textColour ?? Color.Black;
				Font = font;
			}

			public ButtonStyle(Texture2D texture, SpriteFont font, Color? baseColour, Color? hoverColour, Color? textColour) {
				Base = texture;
				Hover = texture;
				BaseColour = baseColour ?? Color.LightGray;
				HoverColour = hoverColour ?? Color.White;
				TextColour = textColour ?? Color.Black;
				Font = font;
			}
		}
	}
}

