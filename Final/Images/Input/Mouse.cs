using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Final.Input {
	﻿
	public class Mouse : GameComponent {
		MouseState current;
		MouseState last;

		public Mouse(Game game) : base(game) {
		}

		/// <summary>
		///     Location of the Mouse as a Point
		/// </summary>
		public Point Location { get { return new Point(current.X, current.Y); } set { Microsoft.Xna.Framework.Input.Mouse.SetPosition(value.X, value.Y); } }

		/// <summary>
		///     Location of the Mouse as a Vector2
		/// </summary>
		public Vector2 LocationV2 { get { return new Vector2(current.X, current.Y); } set { Microsoft.Xna.Framework.Input.Mouse.SetPosition((int)value.X, (int)value.Y); } }

		/// <summary>
		///     Keeps track of the current state of the mouse
		/// </summary>
		public override void Update(GameTime gameTime) {
			last = current;
			current = Microsoft.Xna.Framework.Input.Mouse.GetState();
		}

		/// <summary>
		///     The mouse button is toggled from up to down.
		/// </summary>
		/// <param name="test">The Key to Test</param>
		public Boolean IsButtonToggledDown(MouseButtons test) {
			ButtonSet checkSet = GetValues(test);
			return checkSet.LastButtonState == ButtonState.Released && checkSet.CurrentButtonState == ButtonState.Pressed;
		}

		/// <summary>
		///     The mouse button is toggled from down to up.
		/// </summary>
		/// <param name="test">The Key to Test</param>
		public Boolean IsButtonToggledUp(MouseButtons test) {
			ButtonSet checkSet = GetValues(test);
			return checkSet.LastButtonState == ButtonState.Pressed && checkSet.CurrentButtonState == ButtonState.Released;
		}

		/// <summary>
		///     The mouse button is being pressed.
		/// </summary>
		/// <param name="test">The Key to Test</param>
		public Boolean IsButtonDown(MouseButtons test) {
			return GetValues(test).CurrentButtonState == ButtonState.Pressed;
		}

		/// <summary>
		///     The mouse button is being released.
		/// </summary>
		/// <param name="test">The Key to Test</param>
		public Boolean IsButtonUp(MouseButtons test) {
			return GetValues(test).CurrentButtonState == ButtonState.Released;
		}

		ButtonSet GetValues(MouseButtons test) {
			switch(test) {
				case MouseButtons.Left:
					return new ButtonSet(last.LeftButton, current.LeftButton);
				case MouseButtons.Middle:
					return new ButtonSet(last.MiddleButton, current.MiddleButton);
				case MouseButtons.Right:
					return new ButtonSet(last.RightButton, current.RightButton);
				case MouseButtons.X1:
					return new ButtonSet(last.XButton1, current.XButton1);
				case MouseButtons.X2:
					return new ButtonSet(last.XButton2, current.XButton2);
				default:
					throw new Exception("Unexpected Button" + Environment.NewLine + "Expected Left, Right, Middle, X1, or X2");
			}
		}

		struct ButtonSet {
			internal readonly ButtonState CurrentButtonState;
			internal readonly ButtonState LastButtonState;

			public ButtonSet(ButtonState lastState, ButtonState currentState) {
				LastButtonState = lastState;
				CurrentButtonState = currentState;
			}
		}

		public enum MouseButtons {
			Left,
			Right,
			Middle,
			X1,
			X2
		}
	}
}