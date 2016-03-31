using Microsoft.Xna.Framework;

namespace Edge.Hyperion.Engine {
    public sealed class Circle {
        public int X;
        public int Y;
        public int RadiusX;
        public int RadiusY;
        
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

        public bool Intersects(Rectangle rectangle) {
            // the first thing we want to know is if any of the corners intersect
            var corners = new[]
            {
            new Point(rectangle.Top, rectangle.Left),
            new Point(rectangle.Top, rectangle.Right),
            new Point(rectangle.Bottom, rectangle.Right),
            new Point(rectangle.Bottom, rectangle.Left)
        };

            foreach (var corner in corners) {
                if (ContainsPoint(corner))
                    return true;
            }

            // next we want to know if the left, top, right or bottom edges overlap
            if (X - RadiusX > rectangle.Right || X + RadiusX < rectangle.Left)
                return false;

            if (Y - RadiusY > rectangle.Bottom || Y + RadiusY < rectangle.Top)
                return false;

            return true;
        }

        public bool ContainsPoint(Point point) {
            var vector2 = new Vector2(point.X - X, point.Y - Y);
            return vector2.Length() <= RadiusX;
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
