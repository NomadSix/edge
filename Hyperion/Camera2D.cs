using Microsoft.Xna.Framework;

namespace Edge.Hyperion {
	public class Camera2D {
		public float Zoom;
		public Vector2 Position;

		public Matrix ViewMatrix { 
			get {
				return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
				Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)
				);
			} 
		}

		public Matrix Deproject { 
			get { 
				return Matrix.Invert(
					Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
					Matrix.CreateScale(new Vector3(Zoom, Zoom, 1))
				); 
			} 
		}

		public Camera2D(Vector2 position) {
			Zoom = 1.0f;
			Position = position;
		}
	}
}
