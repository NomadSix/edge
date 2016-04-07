using System;
using Microsoft.Xna.Framework;
using Edge.Hyperion.Engine;

namespace Edge.Hyperion {
    public class Enemy : Entity {
        public long NetID;
        public Type entType;
        public Rectangle hitBox;
        public bool isActive = true;
        public Point Target;
        public Point MoveVector;
        public int currentframe = 0;
        public int mult = 0;

        public Enemy(long id, int x, int y, int mx, int my)
            : base(id, x, y) {
            NetID = id;
            entType = Type.minion;
            X = x;
            Y = y;
            Width = 32;
            Height = 32;
            hitBox = new Rectangle(x, y, Width, Height);
            MoveVector = new Point(mx, my);
        }

        public enum Type:byte {
            minion
        }
    }
}
