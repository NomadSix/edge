using System;
using Microsoft.Xna.Framework;
using Edge.NetCommon;

namespace Edge.Hyperion {
    public class Enemy : Entity {
        public long NetID;
        public Type entType;
        public Enemy(long id, float x, float y) 
            : base(id, x, y) {
            NetID = id;
            entType = Enemy.Type.minion;
            Location = new Vector2(x, y);
        }

        public enum Type:byte {
            minion
        }
    }
}
