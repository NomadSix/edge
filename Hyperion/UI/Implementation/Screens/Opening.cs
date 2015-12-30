using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.UI.Components;
using System.Configuration;
using System;
using Microsoft.Xna.Framework.Input;

namespace Edge.Hyperion.UI.Implementation.Screens {
    public class Opening : Screen{
        Texture2D Logo;
        Single Time = 2f;
        Single Opactity = 0;

        public Opening(Game game, Texture2D logo)
            : base(game) {
                Logo = logo;
        }

        public override void Update(GameTime gameTime) {
            Time -= (1/60f);
            if (Time <= 0 || that.kb.IsAnyKeyDown()) {
                that.SetScreen(new MainMenu(that));
            }
            if (Time > 0) {
                Opactity += (2f/(Time - 1));
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            that.batch.Draw(Logo, new Rectangle(that.GraphicsDevice.Viewport.Width/2, that.GraphicsDevice.Viewport.Height/2, Logo.Width, Logo.Height),
                null, new Color(Color.White, (Int16)Opactity), 0f,
                new Vector2(Logo.Width / 2f, Logo.Height / 2f),
                SpriteEffects.None, 0f);
            that.batch.DrawString(that.Content.Load<SpriteFont>(@"../Font/Helvetica"), "Test", Vector2.Zero, Color.Black);
            base.Draw(gameTime);
            //new Color(255,255,255,Opactity)
            //-Logo.Width / 2, -Logo.Height / 2
        }
    }
}
