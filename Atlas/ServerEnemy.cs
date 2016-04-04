using Microsoft.Xna.Framework;

namespace Edge.Atlas {
    public class ServerEnemy : Entity {
        public long NetID;
        public Point Target;
        public Color pColor;
        public Rectangle Hitbox;
        public Type entType;
        public ServerEnemy(long id, int x, int y, Type enttype) : base(id, x, y) {
            NetID = id;
            Hitbox = new Rectangle(x, y, Width, Height);
            Width = 32;
            Height = 32;
            entType = enttype;
        }

        public enum Type {
            Mage,
            Minion,
            Debug
        }
    }
}