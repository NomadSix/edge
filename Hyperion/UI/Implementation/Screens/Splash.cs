﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.UI.Components;
using System.Configuration;

namespace Edge.Hyperion.UI.Implementation.Screens {
	public class Splash:Screen {
		//Screen saying who made this game, click to start, etc
		Texture2D debugArt;
		float scale;

		public Splash(Game game) : base(game) {
		}

		protected override void LoadContent() {
			debugArt = that.Content.Load<Texture2D>("../Images/Basic_Background");
			base.LoadContent();
		}

		public override void Update(GameTime gameTime) {
			if(scale < .25f) {
				scale += .001f;
			}
			else {
				that.SetScreen(new Gameplay(that, ConfigurationManager.AppSettings["DebugAtlasAddress"], ConfigurationManager.AppSettings["DebugAtlasPort"]));
			}
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {
			that.spriteBatch.Draw(debugArt, Vector2.Zero, null, Color.White, 0f, new Vector2(0f), scale, SpriteEffects.None, 0f);
			base.Draw(gameTime);
		}
	}
}

