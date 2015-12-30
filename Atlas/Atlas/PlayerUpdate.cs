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
	    private Int16 upcount;
	    private Vector2 oldPositon;
	    private Single maxA = 25;

		/// <summary>
		///  Updates a player
		/// </summary>
		/// <param name="player">The player to update</param>
		void PlayerUpdate(DebugPlayer player) {

            //Gravity(player);
            oldPositon = player.Position;
            DebugMove(player);
            Jump(player);
		}
		/// <summary>
		/// Execute the movement logic for the player
		/// Note: This is the old debug logic, and will likely be removed eventually
		/// </summary>
		/// <param name="player">Reference to the player this is being run on</param>
		void DebugMove(DebugPlayer player){
            player.Velocity = player.MoveVector.Y >= 0 ? new Vector2(maxVel * player.MoveVector.X, maxVel * player.MoveVector.Y) : player.Velocity;
		    //Gravity
            player.Velocity.Y += (player.Position.Y + 1f * player.Weight) < Floor ? 1f * player.Weight : 0;
            //Movment
            player.Position += (player.Velocity.Y + player.Position.Y) < Floor ? player.Velocity : Vector2.Zero;
		    if (player.Position.Y >= Floor) player.Position.Y = Floor-1;
		}

	    void Jump(DebugPlayer player) {
	        upcount++;
            if (upcount == 1 && oldPositon.Y == player.Position.Y && player.Velocity.Y >= 0) {
                player.Velocity.Y = -maxA;
	        }
	        else {
	            upcount = 0;
	        }
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

