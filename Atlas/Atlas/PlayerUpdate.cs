using System;
using Microsoft.Xna.Framework;
using System.Diagnostics;


// Analysis disable once CheckNamespace
namespace Edge.Atlas {
	public partial class Atlas {
		public float movespeed = 100;

		/// <summary>
		///  Updates a player
		/// </summary>
		/// <param name="player">The player to update</param>
		void PlayerUpdate(DebugPlayer player) {
			Move(player);
		}
		/*
		void Move(DebugPlayer player) {
			
		}*/
		void Move(DebugPlayer player){
			if(player.Position == player.MovingTo)
				player.MovingTo = new Vector2(-1, -1);
			if(player.MovingTo == new Vector2(-1, -1)) return;
			var deltaY = player.MovingTo.Y - player.Position.Y;
			var deltaX2 = Math.Pow(player.MovingTo.X - player.Position.X, 2);
			var deltaY2 = Math.Pow(deltaY, 2);
			var deltaLen = (float)Math.Sqrt(deltaX2 + deltaY2);
			//Simplified version of cos(arctan(a/b))
			float y = (float)(Math.Sign(deltaY) * (movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond > deltaLen ? deltaLen : movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond) / Math.Sqrt(1 + (deltaX2 / deltaY2)));
			//Simplified version of sin(arctan(a/b))
			float x = (player.MovingTo.X - player.Position.X) * y / (deltaY == 0 ? 1 : deltaY); //TODO: More math way of avoiding division by zero?
			player.Position += new Vector2(x, y);
			/*
			 * Okay, so 4 basic commands
			 * Up, Down, Left, Right
			 * Up=jump/accelerate up
			 * Down=duck/accelerate down
			 * Left=move left
			 * Right=move right
			 * 
			 * Up and down cancel, as do left and right
			 * other directions can combine
			 * 
			 * PACKET TYPES
			 * PositionDelta:
			 * 	Single: XMag
			 * 	Single: YMag
			 */
		}
	}
}

