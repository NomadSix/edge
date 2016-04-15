using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Edge.Hyperion.Engine;
using Button = Edge.Hyperion.UI.Components.Button;

namespace Edge.Hyperion.Backing {
	public static class AssetStore {
		public static SpriteFont FontMain, FontAccent;
		//static place to keep assets
		//TODO: Storage and retrieval list/dictionaries
		public static Dictionary<Button.Style.Type, Button.Style> ButtonTypes = new Dictionary<Button.Style.Type, Button.Style>();
        public static Dictionary<Enemy.Style.Type, Enemy.Style> EnemyTypes = new Dictionary<Enemy.Style.Type, Enemy.Style>();
        public static Dictionary<Item.Style.Type, Item.Style> ItemTypes = new Dictionary<Item.Style.Type, Item.Style>();
        public static SoundEffect MainmenuSong;
        public static Texture2D PlayerTexture, Pixel, Mouse;
        public static Texture2D Sword, Bow, Wand;
        public static Texture2D Ground;
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

