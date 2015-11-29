using System;
using System.Collections.Generic;
using Edge.Hyperion.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Assets = Edge.Hyperion.Backing.AssetStore;

namespace Edge.Hyperion.UI.Implementation.Popups {
    public class PauseMenu : Popup {
        public PauseMenu(Game game, Vector2 location, Int32 width, Int32 height) 
            : base(game, location, width, height) { }

        public override void Initialize() {
            base.Initialize();
        }

        protected override void LoadContent() {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
        }
    }
}
