using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Edge.Hyperion {
	public class Camera2D {
		public float Zoom;
		public Vector2 Position;

		public Matrix ViewMatrix { 
			get {
				return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0))
				* Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
			} 
		}

		public Camera2D(Vector2 position) {
			Zoom = 1.0f;
			Position = position;
		}
	}
}
