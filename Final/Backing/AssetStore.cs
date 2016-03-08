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
		public static Dictionary<Button.Style.Type, Button.Style> ButtonTypes = new Dictionary<Button.Style.Type, Button.Style>();
        public static Texture2D PlayerTexture, Pixel;
        public static Single MasterVolume = 1f;
        public readonly static Byte TileSize = 32;
        public readonly static Int16 TownSize = 100;
        public readonly static Int16 DungenSize = 50;
        public static Random rng = new Random();
        //public static ContentManager Content = new ContentManager(Game);
    }
}

