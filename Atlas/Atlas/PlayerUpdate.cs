using System;
using Microsoft.Xna.Framework;

// Analysis disable once CheckNamespace
namespace Edge.Atlas {
	public partial class Atlas {
		void PlayerUpdate(DebugPlayer player){
			Move(player);
		}

		void Move(DebugPlayer player){
			const int movespeed = 2;
			var deltaX2 = Math.Pow(player.MovingTo.X - player.Position.X, 2);
			var deltaY2 = Math.Pow(player.MovingTo.Y - player.Position.Y, 2);
			var deltaLen = (float) Math.Sqrt(deltaX2 + deltaY2);
			//Okay. Lot going on here. Much optimise. Basically, we're finding out how far we need to go, then converting that to the X component of that using a simplified form of cos(arctan(a/b))
			float x = (float)((movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond > deltaLen ? deltaLen : movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond)/Math.Sqrt(1+(deltaX2/deltaY2)));
			//And here we're just using the fact that cos(arctan(a/b))=a*sin(arctan(a/b))/b
			float y = (player.MovingTo.X - player.Position.X)*x/(player.MovingTo.Y - player.Position.Y);
			player.Position += new Vector2(x, y);
			if(player.Position == player.MovingTo)
				player.MovingTo = new Vector2(-1, -1);
		}
	}
}

