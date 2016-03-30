using Microsoft.Xna.Framework;

namespace Edge.Hyperion.Engine {
    public sealed class Circle {
        public int X;
        public int Y;
        public int RadiusX;
        public int RadiusY;

        public Circle() {
            X = 0;
            Y = 0;
            RadiusX = 0;
            RadiusY = 0;
        }
        public Circle(int x, int y, int radius) {
            X = x;
            Y = y;
            RadiusX = radius;
            RadiusY = radius;
        }
        public Circle(int x, int y, int radiusx, int radiusy) {
            X = x;
            Y = y;
            RadiusX = radiusx;
            RadiusY = radiusy;
        }

        public bool Intersects(Rectangle rec) {
            Point p = new Point();
            p.X = MathHelper.Clamp(getCenter().X, rec.Left, rec.Right);
            p.Y = MathHelper.Clamp(getCenter().Y, rec.Top, rec.Bottom);
            Point direction = getCenter() - p;
            int distanceSquared = (int)direction.ToVector2().LengthSquared();

            return ((distanceSquared > 0) && (distanceSquared < RadiusX * RadiusX));
        }

        public Point getCenter() {
            return new Point(X + RadiusX / 2, Y + RadiusY / 2);
        }

        public Rectangle getRec() {
            return new Rectangle(X, Y, RadiusX, RadiusY);
        }

        public override string ToString() {
            return string.Format("{X = {0}, Y = {1}, RadiusX = {2}, RadiusY = {3}}", X, Y, RadiusX, RadiusY);
        }
    }
}
