using System;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion
{
    public class Entity
    {
        public Vector2 Position;
        public Vector2 MovingTo;
        public Single Velocity;
        public Entity(Int64 id, Single x, Single y) {
			Position = new Vector2(x, y);
		}
    }
}
