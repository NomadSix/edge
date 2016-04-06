using Point = Microsoft.Xna.Framework.Vector2;

namespace Edge.Atlas
{
    public class Entity
    {
        public Point Position;
        public Point MovingTo;
        public Point Velocity;
        public float Health;
        public int Width;
        public int Height;
        public bool remove;
        public Entity(long id, int x, int y) {
			Position = new Point(x, y);
		}
    }
}
