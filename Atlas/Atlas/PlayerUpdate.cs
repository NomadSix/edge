using System;
using Microsoft.Xna.Framework;
using Edge.Atlas.DebugCode;
using System.Threading.Tasks;

// Analysis disable once CheckNamespace
namespace Edge.Atlas {
	public partial class Atlas {
		public float movespeed = 1000;
		public float gravityAccel = 5000;

		/// <summary>
		///  Updates a player
		/// </summary>
		/// <param name="player">The player to update</param>
		void PlayerUpdate(DebugPlayer player) {
			MoveLogic(player);
		}

		/// <summary>
		/// Execute the movement logic for the player
		/// Note: This is the old debug logic, and will likely be removed eventually
		/// </summary>
		/// <param name="player">Reference to the player this is being run on</param>
		//		void DebugMove(DebugPlayer player) {
		//			if(player.Position == player.MovingTo)
		//				player.MovingTo = new Vector2(-1, -1);
		//			if(player.MovingTo == new Vector2(-1, -1)) return;
		//			var deltaY = player.MovingTo.Y - player.Position.Y;
		//			var deltaX2 = Math.Pow(player.MovingTo.X - player.Position.X, 2);
		//			var deltaY2 = Math.Pow(deltaY, 2);
		//			var deltaLen = (float)Math.Sqrt(deltaX2 + deltaY2);
		//			//Simplified version of cos(arctan(a/b))float y = 0;
		//			float y = (float)(Math.Sign(deltaY) * (movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond > deltaLen ? deltaLen : movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond) / Math.Sqrt(1 + (deltaX2 / deltaY2)));
		//			//Simplified version of sin(arctan(a/b))
		//			float x = (player.MovingTo.X - player.Position.X) * y / (deltaY == 0 ? 1 : deltaY);
		//			player.Position += new Vector2(x, y);
		//		}


		void MoveLogic(DebugPlayer player) {
			var colliding = false;
			structures.ForEach(x => {
				//This is gonna be a HUGE slowdown, especially when we have a lot of 
				//If we're on top, Fix the position, and break
			});
			//Also, when we're getting the inputs, set the Y velocity to whatever huge number we want if they're jumping (not 1)
			player.Velocity.Y = colliding ? 0 : player.Velocity.Y + (gravityAccel * (currentTime - lastTime) / TimeSpan.TicksPerSecond);
			player.Position.Y += player.Velocity.Y;
			player.Position.X += player.Velocity.X * movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond;
		}

	}
}

