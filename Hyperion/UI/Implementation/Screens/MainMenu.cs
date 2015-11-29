using System;
using Edge.Hyperion.UI.Components;
using Edge.Hyperion.UI.Implementation.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetStore = Edge.Hyperion.Backing.AssetStore;

namespace Edge.Hyperion.UI.Implementation.Screens {
    
	public class MainMenu:Screen {
		//The main menu, handles interactions with maestro
		//For now just a demo for ui componets
		Button btnPlay;
		//Button btnExit;
		Texture2D backGround;

		public MainMenu(Game game) : base(game) {
			btnPlay = new Button(that, new Rectangle(0, 0, 100, 100), AssetStore.ButtonTypes[Button.ButtonStyle.ButtonStyles.test], "Play", () => that.SetScreen(new Splash(that)));
		}

		public override void Initialize() {
			that.Components.Add(btnPlay);
			base.Initialize();

		}

		protected override void LoadContent() {
			backGround = that.Content.Load<Texture2D>(@"Content/FixMe.png");
			base.LoadContent();
		}

		public override void Update(GameTime gameTime) {

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {
			that.spriteBatch.Draw(backGround, that.bounds, Color.White);
			base.Draw(gameTime);
		}
	}
}

