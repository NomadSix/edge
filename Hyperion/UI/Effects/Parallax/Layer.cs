using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Edge.Hyperion.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Edge.Hyperion.UI.Effects.Parallax {
    public struct Sprite {
        public Texture2D Texture;
        public Vector2 Position;
        private Game that;
        public void Draw(SpriteBatch spriteBatch) {
            if (Texture != null)
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, 0f);
        }
    }
    public class Layer : Screen {
        public Layer(Game game, Camera2D camera) : base(game) {
            _camera = camera;
            that = (Hyperion) game;
            Parallax = Vector2.One;
            Sprites = new List<Sprite>();
        }

        public Vector2 Parallax { get; set; }
        public List<Sprite> Sprites { get; private set; }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Sprite sprite in Sprites)
                sprite.Draw(spriteBatch);
        }

        private readonly Camera2D _camera;
    }
}
