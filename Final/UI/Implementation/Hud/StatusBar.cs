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
        int Height;
        int Width;
        int X, Y;
        Rectangle Bounds;
        Health HealthBar;

        public StatusBar(Game game, float size) : base(game) {
            Size = size;
            HealthBar = new Health(that);
            that.Components.Add(HealthBar);
            X = 8;
            Y = 8;
            Width = 72;
            Height = 72;
        }

        public override void Initialize() {
            Bounds = new Rectangle(X, Y, Width, Height);
            base.Initialize();
        }

        public void draw(float health) {
            that.batch.End();
            that.batch.Begin();
            that.batch.Draw(AssetStore.Sword, Bounds, Color.White);
            HealthBar.draw(health);
            that.batch.End();
        }
    }
}
