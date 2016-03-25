using System;
using Microsoft.Xna.Framework;


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

	}
}

