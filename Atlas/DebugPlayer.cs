using System;
using Microsoft.Xna.Framework;

namespace Edge.Atlas.DebugCode {
    public class DebugPlayer {
        public Int64 NetID;
        public String Name;
		public Vector2 Position;
		public Vector2 MovingTo;
        public Vector2 MoveVector;
        public Vector2 Velocity = new Vector2(0f);
        public Vector2 Acceleration = new Vector2(30f);
        public Single Weight = 2;
		public DebugPlayer(Int64 netid){
			NetID = netid;
		}
	}
}

