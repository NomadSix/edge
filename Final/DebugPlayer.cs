using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Edge.Hyperion.Backing;

namespace Edge.Hyperion {
    public class DebugPlayer : Entity {
        public Vector2 MoveVector;
        public Vector2 Facing;
        public Rectangle AttackRec;
        public Rectangle HitBox;
        public long NetID;
        public byte R = 255;
        public byte G = 255;
        public byte B = 255;
        public string Name;
        public Type entType;

        public DebugPlayer(long id, int x, int y, byte r, byte g, byte b, string name, float health)
            : base(id, x, y) {
            NetID = id;
            entType = Type.Player;
            Name = name.Split(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ' ' })[0];
            X = x;
            Y = y;
            R = r;
            G = g;
            B = b;
            Width = 16;
            Height = 16;
            HitBox = new Rectangle(new Point(X - Width / 2, Y - Height / 2), new Point(Width));
        }

        public enum Type : byte {
            Player
        }
    }
}
