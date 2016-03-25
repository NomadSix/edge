using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Edge.Hyperion.UI.Components;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion.UI.Implementation.Hud
{
    public class StatusBar : UIComponent
    {
        float Size;
        float Height = 75;
        float Width = 0;

        float Health;

        public StatusBar(Game game, float size) : base(game) {
            Size = size;
        }

        public override void Initialize() {
            Height *= Size;
            Width = that.GraphicsDevice.Viewport.Width;
            base.Initialize();
        }

        public void UpdateInfo(DebugPlayer player) {
            Health = player.Health;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
        }
    }
}
