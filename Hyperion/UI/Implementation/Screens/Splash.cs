using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.UI.Components;

namespace Edge.Hyperion.UI.Implementation.Screens {
	public class Splash:Screen {
		//Screen saying who made this game, click to start, etc
        SpriteBatch batch;
        SpriteFont gameFont;
        Game Game;
		public Splash(Game game) : base(game) {
            batch = new SpriteBatch(game.GraphicsDevice);
            Game = game;
            LoadContent();
        }

        void LoadContent() {
            gameFont = Game.Content.Load<SpriteFont>(@"../Fonts/Anonymous.ttf");
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            batch.Begin();
            batch.DrawString(gameFont, "Thing", Vector2.One, Color.White);
            batch.End();
            base.Draw(gameTime);
        }
	}
}

