using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.Input;

namespace Edge.Hyperion.UI.Components {
	public class Slider:UIComponent {
		Rectangle _position;
		Texture2D _background, _knob;
		public Single Value;

		public Slider(Game game, Texture2D background, Texture2D knob, Rectangle location) : base(game) {
			_position = location;
			_background = background;
			_knob = knob;
		}

		public override void Update(GameTime gameTime) {
			if(that.mouse.IsButtonDown(Mouse.MouseButtons.Left) && _position.Contains(that.mouse.Location))
				Value = MathHelper.Clamp(
					(that.mouse.Location.X - (_knob.Width / 2f) - _position.X) / (float)(_position.Width - _knob.Width),
					0, 1);
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {
			that.batch.Draw(_background, _position, Color.White);
			that.batch.Draw(
				_knob,
				new Rectangle(
					(int)(_position.X + (_position.Width - _knob.Width) * Value),
					_position.Y + (_position.Height - _knob.Height) / 2,
					_knob.Width,
					_knob.Height),
				Color.White);
			base.Draw(gameTime);
		}
	}
}