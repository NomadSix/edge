using System;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion {
	public class DebugPlayer {
		public Vector2 Location;
		public Int64 NetID;

		public DebugPlayer(Int64 id, Single x, Single y) {
			NetID = id;
			Location = new Vector2(x, y);
		}
	}
}

