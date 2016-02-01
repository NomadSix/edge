using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Edge.Hyperion.UI.Components;

namespace Edge.Hyperion.BSP {
    public class Dungeon : Screen {
        BspRec tree;

        public Dungeon(Game game) : base(game) {
            tree = new BspRec(0, 0, 100, 100);
            tree.generateDungeon();
        }
    }
}
