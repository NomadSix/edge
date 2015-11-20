using System;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion.UI.Components {
	public class Screen:DrawableGameComponent {
		protected readonly Hyperion that;

		public Screen(Game game) : base(game) {
			that = (Hyperion)game;
		}
	}
}

