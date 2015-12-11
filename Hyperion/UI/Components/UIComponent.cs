using System;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion.UI.Components {
	public class UIComponent:DrawableGameComponent {
		protected Hyperion that;

		public UIComponent(Game game) : base(game) {
			that = (Hyperion)game;
		}
	}
}

