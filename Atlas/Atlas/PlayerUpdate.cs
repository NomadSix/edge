using System;
using Microsoft.Xna.Framework;
using System.Diagnostics;


// Analysis disable once CheckNamespace
namespace Edge.Atlas {
	public partial class Atlas {
		public float movespeed = 20;
		void PlayerUpdate(DebugPlayer player) {
			Move(player);
		}

		void Move(DebugPlayer player) {
			if(player.Position == player.MovingTo)
				player.MovingTo = new Vector2(-1, -1);
			if(player.MovingTo == new Vector2(-1, -1)) return;
			var deltaY = player.MovingTo.Y - player.Position.Y;
			var deltaX2 = Math.Pow(player.MovingTo.X - player.Position.X, 2);
			var deltaY2 = Math.Pow(deltaY, 2);
			var deltaLen = (float)Math.Sqrt(deltaX2 + deltaY2);
			//Okay. Lot going on here. Much optimise. Basically, we're finding out how far we need to go, then converting that to the Y component of that using a simplified form of cos(arctan(a/b))
			float y = (float)(Math.Sign(deltaY) * (movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond > deltaLen ? deltaLen : movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond) / Math.Sqrt(1 + (deltaX2 / deltaY2)));
			//And here we're just using the fact that cos(arctan(a/b))=a*sin(arctan(a/b))/b
			float x = (player.MovingTo.X - player.Position.X) * y / (deltaY==0?1:deltaY); //TODO: More math way of avoiding division by zero?
			player.Position += new Vector2(x, y);

			Console.WriteLine("({0}, {1}) to ({2}, {3})", player.Position.X, player.Position.Y, player.MovingTo.X, player.MovingTo.Y);
		}
	}
}

