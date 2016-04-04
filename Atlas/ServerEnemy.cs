using Microsoft.Xna.Framework;

namespace Edge.Atlas {
    public class ServerEnemy : Entity {
        public long NetID;
        public Point Target;
        public Color pColor;
        public Rectangle Hitbox;
        public ServerEnemy(long id, int x, int y) : base(id, x, y) {
            NetID = id;
            Hitbox = new Rectangle(x, y, Width, Height);
            Width = 32;
            Height = 32;
        }
    }
}