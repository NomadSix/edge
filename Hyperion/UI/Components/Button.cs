using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mouse = Edge.Hyperion.Input.Mouse;

namespace Edge.Hyperion.UI.Components {
	public class Button:UIComponent {
		Boolean _hovering;
        Screen _screen;
		readonly Rectangle _location;
		readonly ButtonStyle _style;
		readonly ButtonAction _action;
		readonly String _text;
		readonly Vector2 _textLocation;

		public delegate void ButtonAction();

		public Button(Game game, Screen screen, Rectangle location, ButtonStyle style, String text, ButtonAction action) : base(game) {
			_location = location;
			_action = action;
			_style = style;
			_text = text;
            _screen = screen;
			//Vector2 measurements = _style.Font.MeasureString(_text);
			//_textLocation = new Vector2(_location.Width / 2f - measurements.X / 2f, _location.Height / 2f - measurements.Y / 2f);
		}

		public override void Update(GameTime gameTime) {
			_hovering = _location.Contains(that.mouse.Location);
			if(_hovering && that.mouse.IsButtonToggledUp(Mouse.MouseButtons.Left) && _screen._isActive)
				_action();
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {
			that.spriteBatch.Draw(_hovering ? _style.Hover : _style.Base, _location, _hovering && _screen._isActive ? _style.HoverColour : _style.BaseColour);
			//that.spriteBatch.DrawString(_style.Font, _text, _textLocation, _style.TextColour);
			base.Draw(gameTime);
		}

		public class ButtonStyle {
			public enum ButtonStyles : byte {
				test
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

