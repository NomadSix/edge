using System;
using Microsoft.Xna.Framework;
using Edge.Hyperion.Engine;

namespace Edge.Hyperion {
    public class Enemy : Entity {
        public long NetID;
        public Type entType;
        public Rectangle hitBox;
        public Circle Vision;
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
            hitBox = new Rectangle(x - Width / 2, y - Height / 2, Width, Height);
            Vision = new Circle(x, y, Backing.AssetStore.TileSize * 4);
        }

        public enum Type:byte {
            minion
        }
    }
}
