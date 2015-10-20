using System;

namespace Edge.Hyperion {
	public class DebugPlayer {
		public Microsoft.Xna.Framework.Vector2 Location;
		public Int64 NetID;
		public DebugPlayer(Int64 id, Single x, Single y) {
			NetID = id;
			Location = new Microsoft.Xna.Framework.Vector2(x, y);
		}
	}
}

