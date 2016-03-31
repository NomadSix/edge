using Microsoft.Xna.Framework;

namespace Edge.Atlas {
    public class ServerEnemy : Entity {
        public long NetID;
        public Point Target;
        public Vector2 Acceleration = new Vector2(30f);
        public Color pColor;
        public ServerEnemy(long id, int x, int y) : base(id, x, y) {
            NetID = id;
        }
    }
}
