using System;
using Edge.Hyperion.UI.Components;
using Edge.Hyperion.UI.Implementation.Popups;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetStore = Edge.Hyperion.Backing.AssetStore;

namespace Edge.Hyperion.UI.Implementation.Screens {
    
	public class MainMenu:Screen {
		//The main menu, handles interactions with maestro
		//For now just a demo for ui componets
		Button btnPlay;
		Button btnAlert;
		Texture2D backGround;

		public MainMenu(Game game) : base(game) {
			btnPlay = new Button(that, this, new Rectangle(0, 0, 100, 50), AssetStore.ButtonTypes[Button.ButtonStyle.ButtonStyles.test], "Play", () => {
                that.SetScreen(new Splash(that));
            });
            btnAlert = new Button(that, this, new Rectangle(125, 0, 100, 50), AssetStore.ButtonTypes[Button.ButtonStyle.ButtonStyles.test], "Alert", () => {
                this._isActive = false;
                that.Components.Add(new Alert(that, this, new Vector2(100, 100), 300, 300));
            });
		}

		public override void Initialize() {
			that.Components.Add(btnPlay);
            that.Components.Add(btnAlert);
			base.Initialize();

		}

		protected override void LoadContent() {
			//backGround = that.Content.Load<Texture2D>(@"../");
			base.LoadContent();
		}

		public override void Update(GameTime gameTime) {

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime) {
			//that.spriteBatch.Draw(backGround, that.bounds, Color.White);
			base.Draw(gameTime);
		}
	}
}

