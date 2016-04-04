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
        int X = 88;
        int Y = 32;
        int Width = 24;
        int Height = 32;
        private int gap = 2;

        public Health(Game game) : base(game) {
            for (int i = 0; i < (int)(2 / .25); i++)
                fullStock.Add(new Rectangle(X + i * gap + i * Width, Y, Width, Height));
        }

        public void update(float health) {
            stock.Clear();
            for (int i = 0; i < (int)(health / .25); i++)
                stock.Add(new Rectangle(X + i * gap + i * Width, Y, Width, Height));
        }

        public void draw(float health) {
            update(health);
            //health stock
            foreach (Rectangle rec in stock) {
                that.batch.Draw(Backing.AssetStore.Pixel, rec, Color.SpringGreen);
                foreach (Rectangle rect in fullStock) {
                    DrawBorder(rect, 3, new Color(50, 50, 50));
                }
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
