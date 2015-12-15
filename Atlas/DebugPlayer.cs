using System;
using Microsoft.Xna.Framework;

namespace Edge.Atlas.DebugCode {
	public class DebugPlayer {
		public Vector2 Position;
		public Vector2 MovingTo;
		public Single Velocity;
		public Int64 NetID;
        public Vector2 MoveVector;
        public Boolean Jump;
		public DebugPlayer(Int64 netid){
			NetID = netid;
		}
	}
}

