using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Edge.Hyperion.UI.Components;
using Microsoft.Xna.Framework;
using Edge.Hyperion.Backing;
using Edge.Hyperion.UI.Implementation.Hud;

namespace Edge.Hyperion.UI.Components {
    public class StatusBar : UIComponent
    {
        float Size;
        float Height = 75;
        int Width = 0;
        int X, Y;
        Rectangle Bounds;
        Health HealthBar;

        public StatusBar(Game game, float size) : base(game) {
            Size = size;
            HealthBar = new Health(that);
            that.Components.Add(HealthBar);
        }

        public override void Initialize() {
            base.Initialize();
        }

        public void draw(float health) {
            HealthBar.draw(health);
        }
    }
}
