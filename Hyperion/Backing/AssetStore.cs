using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Button = Edge.Hyperion.UI.Components.Button;

namespace Edge.Hyperion.Backing {
	public static class AssetStore {
		public static SpriteFont FontMain, FontAccent;
		//static place to keep assets
		//TODO: Storage and retrieval list/dictionaries
		public static Dictionary<Button.ButtonStyle.ButtonStyles, Button.ButtonStyle> ButtonTypes = new Dictionary<Button.ButtonStyle.ButtonStyles, Button.ButtonStyle>();
		//public static ContentManager Content = new ContentManager(Game);
	}
}

