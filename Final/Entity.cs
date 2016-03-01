using System;
using HardShadows;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion {
	public class Entity {
		public Entity(Game game, Int64 id, Single x, Single y) {
			Location = new Vector2(x, y);
            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(x,y);
            points[1] = new Vector2(x,y+16);
            points[2] = new Vector2(x+16,y+16);
            points[3] = new Vector2(x+16,y);
            Hull = new ConvexHull(game, points, Color.Black, new Vector2(x, y));
		}

        private ConvexHull Hull;
	    public Single Health = 1;
		public Vector2 Location;
		public Vector2 Vector;
	}
}
