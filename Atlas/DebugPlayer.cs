using Microsoft.Xna.Framework;

namespace Edge.Atlas {
    public class DebugPlayer : Entity {
        public long NetID;
        public string Name;
        public Color pColor;
        public Rectangle Hitbox;
        public Rectangle Atkbox;
        public Point MoveVector;
        public int mult = 0;
        public int currentFrame = 0;
        public float dmgTimer = 0;
        public float animationTimer = 0;
        public bool isAttacking;
        public bool isDamaged;
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

