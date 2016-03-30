using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Edge.Hyperion.UI.Components;
using Microsoft.Xna.Framework;
using Edge.Hyperion.Backing;

namespace Edge.Hyperion.UI.Components {
    public class StatusBar : UIComponent
    {
        float Size;
        float Height = 75;
        int Width = 0;
        int X, Y;
        Rectangle Bounds;

        float Health;

        public StatusBar(Game game, float size) : base(game) {
            Size = size;
        }

        public override void Initialize() {
            this.Visible = true;
            Height *= Size;
            Width = that.GraphicsDevice.Viewport.Width;
            X = 0;
            Y = that.GraphicsDevice.Viewport.Height - (int)Height;
            Bounds = new Rectangle(X, Y, Width, (int)Height);
            base.Initialize();
        }

        public void UpdateInfo(DebugPlayer player) {
            Health = player.Health;
        }

        public void draw() {
            that.batch.End();
            that.batch.Begin();
            that.batch.Draw(AssetStore.Pixel, Bounds, Color.Gray);
            that.batch.End();
        }
    }
}
