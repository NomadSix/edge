using System;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion.UI.Components {
	public class Screen:UIComponent {
		//TODO: Background texture?
		//TODO: How to handle popup finishing?
		Popup _activePopup;
		public Screen(Game game) : base(game) {}
		public override void Update (GameTime gameTime) {
			base.Update (gameTime);
		}
		protected void SetPopup(Popup popup){
			if (_activePopup == null)
				_activePopup = popup;
		}
	}
}

