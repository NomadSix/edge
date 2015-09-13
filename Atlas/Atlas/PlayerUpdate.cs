using System;
using Microsoft.Xna.Framework;

// Analysis disable once CheckNamespace
namespace Edge.Atlas {
	public partial class Atlas {
		void PlayerUpdate(DebugPlayer player){
			Move(player);
		}

		void Move(DebugPlayer player){
			if(player.MovingTo==new Vector2(-1,-1)) return;
			const float movespeed = 20;
			var deltaY = player.MovingTo.Y - player.Position.Y;
			var deltaX2 = Math.Pow(player.MovingTo.X - player.Position.X, 2);
			var deltaY2 = Math.Pow(player.MovingTo.Y - player.Position.Y, 2);
			var deltaLen = (float) Math.Sqrt(deltaX2 + deltaY2);
			//Okay. Lot going on here. Much optimise. Basically, we're finding out how far we need to go, then converting that to the X component of that using a simplified form of cos(arctan(a/b))
			float y = (float)(Math.Sign(deltaY)*(movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond > deltaLen ? deltaLen : movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond)/Math.Sqrt(1+(deltaX2/deltaY2)));
			//And here we're just using the fact that cos(arctan(a/b))=a*sin(arctan(a/b))/b
			float x = (player.MovingTo.X - player.Position.X)*y/deltaY;
			player.Position += new Vector2(x, y);
			if(player.Position == player.MovingTo)
				player.MovingTo = new Vector2(-1, -1);
			if(Single.IsNaN(player.Position.X) || Single.IsNaN(player.Position.Y)) //TODO: Refactor so this isnt needed
				player.Position = new Vector2(0, 0);
			Console.WriteLine("({0}, {1}) to ({2}, {3})", player.Position.X, player.Position.Y, player.MovingTo.X, player.MovingTo.Y);
		}
	}
}

