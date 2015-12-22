using System;
using Microsoft.Xna.Framework;
using Edge.Atlas.DebugCode;


// Analysis disable once CheckNamespace
namespace Edge.Atlas {
	public partial class Atlas {
        public Single maxVel = 5f;
        public Single friction = 1f;
	    private Boolean Jumping = false;
	    private Int32 Floor = 473;

		/// <summary>
		///  Updates a player
		/// </summary>
		/// <param name="player">The player to update</param>
		void PlayerUpdate(DebugPlayer player) {

            //Gravity(player);
		    DebugMove(player);
		}
		/// <summary>
		/// Execute the movement logic for the player
		/// Note: This is the old debug logic, and will likely be removed eventually
		/// </summary>
		/// <param name="player">Reference to the player this is being run on</param>
		void DebugMove(DebugPlayer player){
            /*
			if(player.Position == player.MovingTo)
				player.MovingTo = new Vector2(-1, -1);
			if(player.MovingTo == new Vector2(-1, -1)) return;
			var deltaY = player.MovingTo.Y - player.Position.Y;
			var deltaX2 = Math.Pow(player.MovingTo.X - player.Position.X, 2);
			var deltaY2 = Math.Pow(deltaY, 2);
			var deltaLen = (float)Math.Sqrt(deltaX2 + deltaY2);
			//Simplified version of cos(arctan(a/b))float y = 0;
            float y = (float)(Math.Sign(deltaY) * (movespeed * (currentTime - lastTime) / 
                TimeSpan.TicksPerSecond > deltaLen ? deltaLen : 
                movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond) / Math.Sqrt(1 + (deltaX2 / deltaY2)));
			//Simplified version of sin(arctan(a/b))
			float x = (player.MovingTo.X - player.Position.X) * y / (deltaY == 0 ? 1 : deltaY);
            if (player.Position.Y + y <= 547 && player.Position.Y + y >= 0 && player.Position.X + x <= 1000)
			    player.Position += new Vector2(x, y);
             */

            player.Velocity = new Vector2(maxVel * player.MoveVector.X, maxVel * player.MoveVector.Y);
            //Gravity
            player.Position.Y += (player.Position.Y + 2f) < Floor ? 2f * player.Weight : 0;
            //Movment
            player.Position += (player.Velocity.Y + player.Position.Y) < Floor ? player.Velocity : Vector2.Zero;
		    if (player.Position.Y >= Floor) player.Position.Y = Floor-1;
		}

        void Gravity(DebugPlayer player) {
            //player.Position += new Vector2(0, 5);
        }

		void MoveLogic(){
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

