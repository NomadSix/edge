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
        int width, height;

        public World(Game game, string csv, int width, int height)
            : base(game)
        {
            this.width = width;
            this.height = height;
        }
    }
}
