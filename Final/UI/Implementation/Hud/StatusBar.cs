using System.Linq;
using System.Text;
using System.Collections.Generic;
using Edge.Hyperion.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        float ControlTimer;
        float scale = 0.5f;

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

        public void draw(DebugPlayer player, GameTime gametime) {
            that.batch.End();
            that.batch.Begin();
            ControlTimer += gametime.ElapsedGameTime.Milliseconds;
            if (ControlTimer <= 5000) {
                DrawCenter("INVINCABLE", 50, Color.Cyan);
                DrawCenter("Controls", 100, Color.White);
                that.batch.Draw(AssetStore.Controls,
                                new Vector2(GraphicsDevice.Viewport.Width / 2 - AssetStore.Controls.Width / 2f * scale,
                                            GraphicsDevice.Viewport.Height / 2 - AssetStore.Controls.Height / 2f * scale),
                                null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f
                );
                scale -= 1 / 625f;
            }
            that.batch.Draw(AssetStore.Sword, Bounds, Color.White);
            that.batch.DrawString(AssetStore.FontMain, "Gold: " + player.gold.ToString(), new Vector2(Bounds.X, Bounds.Y + Bounds.Height + 10), Color.White);
            HealthBar.draw(player.Health);
            that.batch.End();
        }

        public void DrawCenter(string text, int y, Color color) {
            var viewport = GraphicsDevice.Viewport;
            var measure = that.Helvetica.MeasureString(text);
            //var location = Vector2.Transform(new Vector2(viewport.Width / 2 - measure.X / 2, 50), cam.ViewMatrix);
            that.batch.DrawString(that.Helvetica, text, new Vector2(viewport.Width / 2 - measure.X / 2, y), color);
        }
    }
}
