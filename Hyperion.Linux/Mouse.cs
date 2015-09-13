using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Edge.Hyperion.Input {
	﻿
	public class Mouse : GameComponent {
		MouseState current;
		MouseState last;

		public Mouse(Game game) : base(game) {
		}

		/// <summary>
		///     Location of the Mouse as a Point
		/// </summary>
		public Point Location { get { return new Point(current.X, current.Y); } }

		/// <summary>
		///     Location of the Mouse as a Vector2
		/// </summary>
		public Vector2 LocationV2 { get { return new Vector2(current.X, current.Y); } }

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

		private ButtonSet GetValues(MouseButtons test) {
			ButtonState lastButtonState;
			ButtonState currentButtonState;
			switch(test) {
				case MouseButtons.Left:
					lastButtonState = last.LeftButton;
					currentButtonState = current.LeftButton;
					break;
				case MouseButtons.Middle:
					lastButtonState = last.MiddleButton;
					currentButtonState = current.MiddleButton;
					break;
				case MouseButtons.Right:
					lastButtonState = last.RightButton;
					currentButtonState = current.RightButton;
					break;
				case MouseButtons.X1:
					lastButtonState = last.XButton1;
					currentButtonState = current.XButton1;
					break;
				case MouseButtons.X2:
					lastButtonState = last.XButton2;
					currentButtonState = current.XButton2;
					break;
				default:
					throw new Exception("Unexpected Button" + Environment.NewLine + "Expected Left, Right, Middle, X1, or X2");
			}
			return new ButtonSet(lastButtonState, currentButtonState);
		}

		struct ButtonSet {
			internal readonly ButtonState CurrentButtonState;
			internal readonly ButtonState LastButtonState;

			public ButtonSet(ButtonState lastState, ButtonState currentState) {
				LastButtonState = lastState;
				CurrentButtonState = currentState;
			}
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