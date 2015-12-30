using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Edge.Hyperion {
	public class Camera2D {
		public float Zoom;
		public Vector2 Position;
	    public Vector2 Origin;
	    private Viewport viewport;

		public Matrix ViewMatrix { 
			get {
			    return Matrix.CreateTranslation(new Vector3(-Position, 0f))*
                       Matrix.CreateTranslation(new Vector3(-Origin, 0f)) *
			           Matrix.CreateScale(new Vector3(Zoom, Zoom, 1))*
                       Matrix.CreateTranslation(new Vector3(Origin, 0f));
			} 
		}

		public Matrix Deproject { 
			get { 
				return Matrix.Invert(
                       Matrix.CreateTranslation(new Vector3(-Position, 0f)) *
                       Matrix.CreateTranslation(new Vector3(-Origin, 0f)) *
					   Matrix.CreateScale(new Vector3(Zoom, Zoom, 1f)) *
                       Matrix.CreateTranslation(new Vector3(Origin, 0f))
                );
			} 
		}

		public Camera2D(Vector2 position, GraphicsDevice graphics) {
			Zoom = 1.0f;
			Position = position;
		    viewport = graphics.Viewport;
            Origin = new Vector2(viewport.Width/2f, viewport.Height/2f);
		}
	}
}
