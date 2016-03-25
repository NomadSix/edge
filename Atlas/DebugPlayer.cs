using Microsoft.Xna.Framework;

namespace Edge.Atlas {
    public class DebugPlayer : Entity {
        public long NetID;
        public string Name;
        public Vector2 MoveVector;
        public Vector2 Acceleration = new Vector2(30f);
        public Color pColor;
		public DebugPlayer(long id, float x, float y, float health) : base(id, x, y){
			NetID = id;
            Position = new Vector2(x, y);
            Health = health;
		}
	}
}

