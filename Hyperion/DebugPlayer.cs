using System;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion {
	public class DebugPlayer {
		public Vector2 Location;
	    public Vector2 MoveVector;
	    public String Name;
		public Int64 NetID;

		public DebugPlayer(Int64 id, Single x, Single y, String name) {
			NetID = id;
		    Name = name;
			Location = new Vector2(x, y);
		}
	}
}

