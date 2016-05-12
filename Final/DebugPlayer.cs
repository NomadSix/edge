using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Edge.Hyperion.Backing;
using System.Collections.Generic;

namespace Edge.Hyperion {
    public class DebugPlayer : Entity {
        public Point MoveVector;
        public Rectangle AttackRec;
        public Rectangle HitBox;
        public long NetID;
        public string Name;
        public Type entType;
        public float lifeTimer = 0;
        public int currentFrame = 0;
        public int mult = 0;
        public int gold = 0;
        public bool isAttacking;
        public bool isDamaged;

        public DebugPlayer(long id, int x, int y, string name, float health)
            : base(id, x, y) {
            NetID = id;
            entType = Type.Player;
            Name = name.Split(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ' ' })[0];
            X = x;
            Y = y;
            Width = 14;
            Height = 16;
            Health = health;
            HitBox = new Rectangle(X, Y, Width, Height);
        }

        public enum Type : byte {
            Player
        }
    }
}
