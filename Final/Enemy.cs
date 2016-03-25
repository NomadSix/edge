using System;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion {
    public class Enemy : Entity {
        public long NetID;
        public Enemy(long id, float x, float y) 
            : base(id, x, y) {
            NetID = id;
            entType = Type.Enemy;
            Location = new Vector2(x, y);
        }
    }
}
