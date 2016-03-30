using Point = Microsoft.Xna.Framework.Point;

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
        public Entity(long id, int x, int y) {
			Position = new Point(x, y);
		}
    }
}
