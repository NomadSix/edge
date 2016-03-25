using System;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion {
	public class Entity {
        public enum Type {
            Player,
            Enemy
        }
        public Entity(Int64 id, Single x, Single y) {
			Location = new Vector2(x, y);
		}
	    public float Health = 1;
		public Vector2 Location;
        public Type entType;
    }
}
