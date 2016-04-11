using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edge.Hyperion.Engine {
    public class Item {
        public long ID;
        public Point Position;
        public int Width;
        public int Height;
        public bool remove;
        public Rectangle Hitbox;
        public Style type;
        public Item(long id, int x, int y, Style style) {
            ID = id;
            type = style;
            Position = new Point(x, y);
            Width = 16;
            Height = 16;
            Hitbox = new Rectangle(x, y, Width, Height);
        }

        public class Style {

            public enum Type {
                Health
            }
            public Texture2D Base;

            public Style(Texture2D texture) {
                Base = texture;
            }
        }
    }
}
