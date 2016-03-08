using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Edge.Hyperion.UI.Components;
using AssetStore = Edge.Hyperion.Backing.AssetStore;

namespace Edge.Hyperion.UI.Implementation.Popups {
	class Alert:Popup {
        Button btnOk;
        Screen Screen;

		public Alert(Game game, Screen screen, int x, int y, int width, int height) 
            : base(game, x, y, width, height) {
                Screen = screen;
		}

        public override void Initialize() {
            btnOk = new Button(that, this, new Rectangle(100, 150, 100, 50), AssetStore.ButtonTypes[Button.Style.Type.basic], "OK", () => {
                Screen._isActive = true;
                that.Components.Remove(btnOk);
                Kill();
            });
            that.Components.Add(btnOk);
 	        base.Initialize();
        }

        public override void Draw(GameTime gameTime) {
            that.batch.Draw(backGround, new Vector2(100, 75), null, new Color(50, 50, 50, 100), 0f, Vector2.Zero, new Vector2(100, 100), SpriteEffects.None, 0f);
            base.Draw(gameTime);
        }
	}
}
