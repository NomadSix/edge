using System;
using Microsoft.Xna.Framework;

namespace Edge.Atlas {
	public class DebugPlayer {
		public Vector2 Position;
		public Vector2 MovingTo;
		public Single Velocity;
		public Int64 NetID;
		public DebugPlayer(Int64 netid){
			NetID = netid;
		}
	}
}

