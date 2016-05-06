using Microsoft.Xna.Framework;

namespace Edge.Atlas {
    public class DebugPlayer : Entity {
        public long NetID;
        public string Name;
        public Color pColor;
        public Rectangle Hitbox;
        public Rectangle Atkbox;
        public Point MoveVector;
        public int   mult;
        public int   currentFrame;
        public int   gold;
        public float dmgTimer;
        public float animationTimer;
        public float attackTimer;
        public bool  isAttacking;
        public bool  isDamaged;
        public DebugPlayer(long id, int x, int y, float health) : base(id, x, y){
			NetID = id;
            Health = health;
            Position = new Vector2(x, y);
            Width = 14;
            Height = 16;
            world = World.debug;
        }
	}
}

