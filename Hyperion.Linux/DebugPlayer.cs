using System;

namespace Edge.Hyperion {
	public class DebugPlayer {
		public Microsoft.Xna.Framework.Point Location;
		public Int64 NetID;
		public DebugPlayer(Int64 id, UInt16 x, UInt16 y) {
			NetID = id;
			Location = new Microsoft.Xna.Framework.Point(x, y);
		}
	}
}

