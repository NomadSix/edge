using System;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion {
    public class DebugPlayer : Entity {
        public Vector2 Location;
        public Vector2 MoveVector;
        public Int64 NetID;
        public string Name;

        public DebugPlayer(Game game, Int64 id, float x, float y, string name)
            : base(game, id, x, y) {
            NetID = id;
            Name = name;
            Location = new Vector2(x, y);
        }
    }
}
