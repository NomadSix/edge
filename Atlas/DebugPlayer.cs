using Microsoft.Xna.Framework;

namespace Edge.Atlas {
    public class DebugPlayer : Entity {
        public long NetID;
        public string Name;
        public Vector2 MoveVector;
        public Vector2 Acceleration = new Vector2(30f);
        public Color pColor;
		public DebugPlayer(long id, int x, int y, float health) : base(id, x, y){
			NetID = id;
            Position = new Point(x, y);
            Health = health;
		}
	}
}

