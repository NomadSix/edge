using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.UI.Components;
using System.Configuration;
using System;

namespace Edge.Hyperion.UI.Implementation.Screens {
    public class Opening : Screen{
        Texture2D Logo;
        Single Time = 4;
        Int32 Opactity = 0;

        public Opening(Game game, Texture2D logo)
            : base(game) {
                Logo = logo;
        }

        public override void Update(GameTime gameTime) {
            Time -= (1/60f);
            if (Time > 0) {
                Opactity += 2;
            } else {
                that.SetScreen(new MainMenu(that));
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            that.spriteBatch.Draw(Logo, new Rectangle(that.GraphicsDevice.Viewport.Width/2, that.GraphicsDevice.Viewport.Height/2, Logo.Width, Logo.Height),
                null, new Color(Color.DarkGray, Opactity), 0f,
                new Vector2(Logo.Width / 2, Logo.Height / 2),
                SpriteEffects.None, 0f);
            that.spriteBatch.DrawString(that.Content.Load<SpriteFont>(@"../Font/Helvetica"), "Test", Vector2.Zero, Color.Black);
            base.Draw(gameTime);
            //new Color(255,255,255,Opactity)
            //-Logo.Width / 2, -Logo.Height / 2
        }
    }
}
