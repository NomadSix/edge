using System;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion {
	public class Entity {
        public Entity(Int64 id, Single x, Single y) {
			Location = new Vector2(x, y);
		}
	    public float Health = 1;
		public Vector2 Location;
        public int Width;
        public int Height;
    }
}
