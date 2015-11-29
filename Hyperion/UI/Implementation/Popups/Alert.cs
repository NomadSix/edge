using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Edge.Hyperion.UI.Components;
using AssetStore = Edge.Hyperion.Backing.AssetStore;

namespace Edge.Hyperion.UI.Implementation.Popups {
	class Alert:Popup {
        Button btnOk;
        Texture2D backGround;
        Screen Screen;
		public Alert(Game game, Screen screen, Vector2 location, Int32 width, Int32 height) 
            : base(game, location, width, height) {
                Screen = screen;
		}
        public override void Initialize() {
            btnOk = new Button(that, this, new Rectangle(100, 150, 100, 50), AssetStore.ButtonTypes[Button.ButtonStyle.ButtonStyles.test], "OK", () => {
                Screen._isActive = true;
                that.Components.Remove(btnOk);
                that.Components.Remove(this);
            });
            that.Components.Add(btnOk);
 	        base.Initialize();
        }
        protected override void LoadContent() {
            backGround = that.Content.Load<Texture2D>(@"../Images/Grey.png");
            base.LoadContent();
        }
        public override void Draw(GameTime gameTime) {
            that.spriteBatch.Draw(backGround, new Vector2(100, 75), null, new Color(50, 50, 50, 100), 0f, Vector2.Zero, new Vector2(100, 100), SpriteEffects.None, 0f);
            base.Draw(gameTime);
        }
	}
}
