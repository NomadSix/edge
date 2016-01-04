using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Edge.Hyperion.UI.Effects.Parallax {
    public class Background
    {
        private Texture2D Texture;      //The image to use
        private Vector2 Offset;         //Offset to start drawing our image
        public Vector2 Speed;           //Speed of movement of our parallax effect
        public float Zoom;              //Zoom level of our image
 
        private Viewport Viewport;      //Our game viewport
        private Camera2D Cam;
 
        //Calculate Rectangle dimensions, based on offset/viewport/zoom values
        private Rectangle Rectangle
        {
            get { return new Rectangle((int)(Offset.X), (int)(Offset.Y), (int)(Viewport.Width / Zoom), (int)(Viewport.Height / Zoom)); }
        }
 
        public Background(Texture2D texture, Vector2 speed, float zoom)
        {
            Texture = texture;
            Offset = Vector2.Zero;
            Speed = speed;
            Zoom = zoom;
        }
 
        public void Update(GameTime gametime, Vector2 direction, Viewport viewport, Camera2D cam)
        {
            float elapsed = (float)gametime.ElapsedGameTime.TotalSeconds;
 
            //Store the viewport
            Viewport = viewport;
            Cam = cam;
            //Calculate the distance to move our image, based on speed
            Vector2 distance = direction * Speed * elapsed;       
 
            //Update our offset
            Offset += distance;
        }
 
        public void Draw(SpriteBatch spriteBatch, Camera2D cam) {
            Cam = cam;
            spriteBatch.Draw(Texture, Cam.Position, Rectangle, Color.White, 0, Vector2.Zero, Zoom, SpriteEffects.None, 1);
        }
    }
}
