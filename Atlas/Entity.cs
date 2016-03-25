using Microsoft.Xna.Framework;

namespace Edge.Atlas
{
    public class Entity
    {
        public Vector2 Position;
        public Vector2 MovingTo;
        public Vector2 Velocity;
        public float Health;
        public int Width;
        public int Height;
        public Entity(long id, float x, float y) {
			Position = new Vector2(x, y);
		}
    }
}
