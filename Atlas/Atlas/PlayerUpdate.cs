using System;
using Microsoft.Xna.Framework;
using Edge.Atlas.DebugCode;
using OpenTK.Graphics.OpenGL;
using Boolean = System.Boolean;


// Analysis disable once CheckNamespace
namespace Edge.Atlas {
	public partial class Atlas {
        private Vector2 maxVel = new Vector2(2f,2f);

		/// <summary>
		///  Updates a player
		/// </summary>
		/// <param name="player">The player to update</param>
		void PlayerUpdate(DebugPlayer player) {

            //Gravity(player);
            DebugMove(player);
            //MoveTo(player, 200);
		}
		/// <summary>
		/// Execute the movement logic for the player
		/// Note: This is the old debug logic, and will likely be removed eventually
		/// </summary>
		/// <param name="player">Reference to the player this is being run on</param>
		void DebugMove(DebugPlayer player) {
		    var dt = (currentTime - lastTime)/TimeSpan.TicksPerMillisecond;

            if (player.MoveVector.Y >= 0)
                player.Velocity.Y = maxVel.Y * player.MoveVector.Y;
            if (player.MoveVector.Y <= 0)
                player.Velocity.Y = maxVel.Y * player.MoveVector.Y;

            if (player.MoveVector.X >= 0)
                player.Velocity.X = maxVel.X * player.MoveVector.X;
            if (player.MoveVector.X <= 0)
                player.Velocity.X = maxVel.X * player.MoveVector.X;
            player.Position += player.Velocity;
        }

        void MoveTo(DebugPlayer player, Single movespeed) {
            if (player.Position == player.MovingTo)
                player.MovingTo = new Vector2(-1, -1);
            if (player.MovingTo == new Vector2(-1, -1)) return; 
            var deltaY = player.MovingTo.Y - player.Position.Y;
            var deltaX2 = Math.Pow(player.MovingTo.X - player.Position.X, 2);
            var deltaY2 = Math.Pow(deltaY, 2);
            var deltaLen = (float)Math.Sqrt(deltaX2 + deltaY2);
            //Simplified version of cos(arctan(a/b))float y = 0;
            float y = (float)(Math.Sign(deltaY) * (movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond > deltaLen ? deltaLen : movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond) / Math.Sqrt(1 + (deltaX2 / deltaY2)));
            //Simplified version of sin(arctan(a/b))
            float x = (player.MovingTo.X - player.Position.X) * y / (deltaY == 0 ? 1 : deltaY);
            player.Position += new Vector2(x, y);
	    }

	}
}

