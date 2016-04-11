using Microsoft.Xna.Framework;
using Point = Microsoft.Xna.Framework.Vector2;

namespace Edge.Atlas {
    public class Item {
        public long ID;
        public Point Position;
        public int Width;
        public int Height;
        public bool remove;
        public Rectangle Hitbox;
        public Type type;
        public Item(long id, int x, int y, Type type) {
            ID = id;
            this.type = type;
            Position = new Point(x, y);
            Width = 16;
            Height = 16;
            Hitbox = new Rectangle(x, y, Width, Height);
        }

        public enum Type {
            Health
        }
    }
}
