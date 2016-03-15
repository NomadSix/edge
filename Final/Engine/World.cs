using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edge.Hyperion.UI.Components;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion.Engine
{
    public class World : Screen
    {
        TileMap csv;
        int width, height;

        public World(Game game, string csv, int width, int height)
            : base(game)
        {
            this.csv = new TileMap(csv);
            this.width = width;
            this.height = height;
        }
    }
}
