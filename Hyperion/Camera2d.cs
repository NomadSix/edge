using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Edge.Hyperion
{
    public class Camera2d
    {
        protected float zoom;
        public Matrix transloation;
        public Vector2 pos;

        public Camera2d()
        {
            zoom = 1.0f;
            pos = Vector2.Zero;
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; }
        }

        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public Matrix getTransforamtion(GraphicsDevice graphicsDevice)
        {
            return transloation = Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) * Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
        }
    }
}
