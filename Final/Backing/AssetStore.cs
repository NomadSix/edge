using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Button = Edge.Hyperion.UI.Components.Button;

namespace Edge.Hyperion.Backing {
	public static class AssetStore {
		public static SpriteFont FontMain, FontAccent;
		//static place to keep assets
		//TODO: Storage and retrieval list/dictionaries
		public static Dictionary<Button.Style.Type, Button.Style> ButtonTypes = new Dictionary<Button.Style.Type, Button.Style>();
        public static Texture2D PlayerTexture, Pixel, Mouse;
        public static Texture2D Sword, Bow, Wand;
        public static float MasterVolume = 1f;
        public readonly static byte TileSize = 32;
        public readonly static short TownSize = 50;
        public readonly static short DungenSize = 50;
        public static Random rng = new Random();
        public static int Width = 1280;
        public static int Height = 720;

        internal static Keyboard kb;
        internal static Mouse mouse;
        //public static ContentManager Content = new ContentManager(Game);
    }
}

