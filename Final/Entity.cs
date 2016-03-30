using System;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion {
	public class Entity {
        public Entity(long id, int x, int y) {
            X = x;
            Y = y;
		}
	    public float Health = 1;
        public int X;
        public int Y;
        public int Width;
        public int Height;
    }
}
