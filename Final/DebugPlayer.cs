using System;
using Microsoft.Xna.Framework;
using Edge.Hyperion.Backing;

namespace Edge.Hyperion {
    public class DebugPlayer : Entity {
        public Vector2 Location;
        public Vector2 MoveVector;
        public Vector2 Facing;
        public Rectangle AttackRec;
        public Rectangle HitBox;
        public Int64 NetID;
        public byte R = 255;
        public byte G = 255;
        public byte B = 255;
        public string Name;

        public DebugPlayer(Game game, Int64 id, float x, float y, byte r, byte g, byte b, string name)
            : base(game, id, x, y) {
            NetID = id;
            Name = name.Split(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ' ' })[0];
            Location = new Vector2(x, y);
            HitBox = new Rectangle(Location.ToPoint(), new Point(32));
            R = r;
            G = g;
            B = b;
        }
    }
}
