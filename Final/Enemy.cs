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
        public Enemy(long id, int x, int y)
            : base(id, x, y) {
            NetID = id;
            entType = Type.minion;
            X = x;
            Y = y;
            Width = 32;
            Height = 32;
            hitBox = new Rectangle(x, y, Width, Height);
        }

        public enum Type:byte {
            minion
        }
    }
}
