using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion.Engine {
    static class Tile {
        static public Texture2D TileSetTexture;

        static public Rectangle GetScorceRectangle(int titleIndex) {
            return new Rectangle(titleIndex * 32, 0, 32, 32);
        }

        public enum Type {
            Soild,
            Air
        }
    }
}
