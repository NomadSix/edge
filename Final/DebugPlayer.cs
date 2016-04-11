using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Edge.Hyperion.Backing;

namespace Edge.Hyperion {
    public class DebugPlayer : Entity {
        public Point MoveVector;
        public Rectangle AttackRec;
        public Rectangle HitBox;
        public long NetID;
        public byte R = 255;
        public byte G = 255;
        public byte B = 255;
        public string Name;
        public Type entType;
        public int mult = 0;
        public int currentFrame = 0;
        public bool isAttacking;
        public bool isDamaged;

        public DebugPlayer(long id, int x, int y, string name, float health)
            : base(id, x, y) {
            NetID = id;
            entType = Type.Player;
            Name = name.Split(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ' ' })[0];
            X = x;
            Y = y;
            Width = 16;
            Height = 16;
            Health = health;
            HitBox = new Rectangle(new Point(X, Y), new Point(Width));
        }

        public enum Type : byte {
            Player
        }
    }
}
