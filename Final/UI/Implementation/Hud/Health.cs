using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edge.Hyperion.UI.Components;
using Microsoft.Xna.Framework;

namespace Edge.Hyperion.UI.Implementation.Hud {
    public class Health : UIComponent{

        List<Rectangle> stock = new List<Rectangle>();
        List<Rectangle> fullStock = new List<Rectangle>();

        public Health(Game game) : base(game) {
            for (int i = 0; i < 4; i++)
                fullStock.Add(new Rectangle((88) + i * 8 + i * 24, 16, 24, 32));
        }

        public void update(float health) {
            stock.Clear();
            for (int i = 0; i < (int)(health / .25); i++)
                stock.Add(new Rectangle((88) + i * 8 + i * 24, 16, 24, 32));
        }

        public void draw(float health) {
            update(health);
            that.batch.End();
            that.batch.Begin();
            //health stock
            foreach (Rectangle rec in stock) {
                that.batch.Draw(Backing.AssetStore.Pixel, rec, Color.SpringGreen);
            }
            foreach (Rectangle rec in fullStock) {
                DrawBorder(rec, 3, Color.Black);
            }
            that.batch.End();
        }
        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor) {
            // Draw top line
            that.batch.Draw(Backing.AssetStore.Pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            that.batch.Draw(Backing.AssetStore.Pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            that.batch.Draw(Backing.AssetStore.Pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            that.batch.Draw(Backing.AssetStore.Pixel, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }
    }
}
