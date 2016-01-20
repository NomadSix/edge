using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Edge.Hyperion.UI.Implementation.Screens;

namespace Edge.Hyperion {
    public class Player : Entity {

        Vector2 PlayerArms;
        Texture2D Image;
        Hyperion that;
        Gameplay Game;

        public Player(Gameplay game, Int64 id, Single x, Single y, Texture2D image, Hyperion that) : base (id, x, y) {
            Location = new Vector2(x, y);
            PlayerArms = new Vector2(x + image.Width/2, y + image.Height/2);
            Image = image;
            this.that = that;
            Game = game;
        }

        public void Update() {
        }

        public void Draw() { }
    }
}
