using System;
using Microsoft.Xna.Framework;

namespace Edge.Atlas {
	public class Entity {
		public Int64 ID;
		public Vector2 Position;
		public Vector2 MovingTo;
		public Single YVelocity;
		public Boolean Solid;

		public Entity(Int64 id, Vector2 position, Boolean solid = true) {
			ID = id;
			Position = position;
			Solid = solid;
		}
	}
}
