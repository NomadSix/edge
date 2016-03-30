using System;
using Microsoft.Xna.Framework;
using Edge.NetCommon;

namespace Edge.Hyperion {
    public class Enemy : Entity {
        public long NetID;
        public Type entType;
        public Rectangle hitBox;
        public bool isActive = true;
        public Enemy(long id, int x, int y)
            : base(id, x, y) {
            NetID = id;
            entType = Type.minion;
            X = x;
            Y = y;
            Width = 32;
            Height = 32;
            hitBox = new Rectangle(x - Width / 2, y - Height / 2, Width, Height);
        }

        public enum Type:byte {
            minion
        }
    }
}
