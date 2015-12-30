using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edge.Hyperion.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Edge.Hyperion.UI.Effects {
    public class Parallax : Screen {
        private List<Texture2D> Layers = new List<Texture2D>(); 
        private const Single Rate = .1f;

        public Parallax(Game game, List<Texture2D> layers) : base(game) {
            Layers = layers;
        }

        public override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
            
        }
    }
}
