using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Final.Input {
	public class Keyboard : GameComponent {
		KeyboardState current;
		KeyboardState last;

		public Keyboard(Game game) : base(game) {
		}

		/// <summary>
		/// Gets the current text input by the keyboard
		/// </summary>
		/// <value>The message</value>
		public string Message { get; private set; }

		/// <summary>
		/// -Get this frame's current state
		/// -Update the message
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public override void Update(GameTime gameTime) {
			last = current;
			current = Microsoft.Xna.Framework.Input.Keyboard.GetState();
			foreach(Keys x in current.GetPressedKeys().Except(last.GetPressedKeys())) {
				string keyString = string.Empty;
				bool isShiftDown = current.IsKeyDown(Keys.LeftShift) || current.IsKeyDown(Keys.RightShift);
				switch(x) {
					case Keys.Back:
						Message = (Message.Length > 1) ? Message.Remove(Message.Length - 1) : "";
						return;
					case Keys.D1:
						keyString = isShiftDown ? "!" : "1";
						break;
					case Keys.D2:
						keyString = isShiftDown ? "@" : "2";
						break;
					case Keys.D3:
						keyString = isShiftDown ? "#" : "3";
						break;
					case Keys.D4:
						keyString = isShiftDown ? "$" : "4";
						break;
					case Keys.D5:
						keyString = isShiftDown ? "%" : "5";
						break;
					case Keys.D6:
						keyString = isShiftDown ? "^" : "6";
						break;
					case Keys.D7:
						keyString = isShiftDown ? "&" : "7";
						break;
					case Keys.D8:
						keyString = isShiftDown ? "*" : "8";
						break;
					case Keys.D9:
						keyString = isShiftDown ? "(" : "9";
						break;
					case Keys.D0:
						keyString = isShiftDown ? ")" : "0";
						break;
					case Keys.OemMinus:
						keyString = isShiftDown ? "_" : "-";
						break;
					case Keys.OemPlus:
						keyString = isShiftDown ? "+" : "=";
						break;
					case Keys.OemOpenBrackets:
						keyString = isShiftDown ? "{" : "[";
						break;
					case Keys.OemCloseBrackets:
						keyString = isShiftDown ? "}" : "]";
						break;
					case Keys.OemPipe:
						keyString = isShiftDown ? "|" : "\\";
						break;
					case Keys.OemSemicolon:
						keyString = isShiftDown ? ":" : ";";
						break;
					case Keys.OemQuotes:
						keyString = isShiftDown ? "\"" : "'";
						break;
					case Keys.OemComma:
						keyString = isShiftDown ? "<" : ",";
						break;
					case Keys.OemPeriod:
						keyString = isShiftDown ? ">" : ".";
						break;
					case Keys.OemQuestion:
						keyString = isShiftDown ? "?" : "/";
						break;
					case Keys.OemTilde:
						keyString = isShiftDown ? "~" : "`";
						break;
					case Keys.Space:
						keyString = " ";
						break;
					case Keys.Enter:
						keyString = Environment.NewLine;
						keyString = "";
						break;
					default:
						if(x != Keys.LeftShift && x != Keys.RightShift) {
							string temp = Regex.IsMatch(x.ToString(), @"\d")
								? Regex.Match(x.ToString(), @"\d+").Value : x.ToString();
							keyString = isShiftDown ? temp.ToUpper() : temp.ToLower();
						}
						break;
				}
				Message += keyString.Length > 1 ? (keyString.Remove(1, (keyString.Length) - 1)) : keyString;
			}
		}

		/// <summary>
		/// Clears the input message
		/// </summary>
		public void ResetString() {
			Message = String.Empty;
		}

		/// <summary>
		/// Determines if this button toggled from up to down
		/// </summary>
		/// <returns>Whether it was toggled down or not</returns>
		/// <param name="test">The button to test</param>
		public Boolean IsButtonToggledDown(Keys test) {
			return current.IsKeyDown(test) && last.IsKeyUp(test);
		}

		/// <summary>
		/// Determines if this button toggled from down to up
		/// </summary>
		/// <returns>Whether it was toggled up or not</returns>
		/// <param name="test">The button to test</param>
		public Boolean IsButtonToggledUp(Keys test) {
			return current.IsKeyUp(test) && last.IsKeyDown(test);
		}

		/// <summary>
		/// Determines if this button toggled from up to down
		/// </summary>
		/// <returns>Whether it is down or not</returns>
		/// <param name="test">The button to test</param>
		public Boolean IsButtonDown(Keys test) {
			return current.IsKeyDown(test);
		}

		/// <summary>
		/// Determines if this button is up
		/// </summary>
		/// <returns>Whether it is up or not</returns>
		/// <param name="test">The button to test</param>
		public Boolean IsButtonUp(Keys test) {
			return current.IsKeyUp(test);
		}

	    public Boolean IsAnyKeyDown() {
	        return current.GetPressedKeys().Length > 0;
	    }
	}
}