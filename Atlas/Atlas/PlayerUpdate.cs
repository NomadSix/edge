using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


// Analysis disable once CheckNamespace
namespace Edge.Atlas {
	public partial class Atlas {
        private Vector2 maxVel = new Vector2(150f);
        private List<ServerEnemy> ent;
        private float timer = 0;

		/// <summary>
		///  Updates a player
		/// </summary>
		/// <param name="player">The player to update</param>
		void PlayerUpdate(DebugPlayer player, List<ServerEnemy> enemy) {
            ent = enemy;
            //Gravity(player);
            DebugMove(player);
            //MoveTo(player, 200);
		}
		/// <summary>
		/// Execute the movement logic for the player+		enemys[0].Target	'enemys[0].Target' threw an exception of type 'System.Collections.Generic.KeyNotFoundException'	Microsoft.Xna.Framework.Point {System.Collections.Generic.KeyNotFoundException}

		/// Note: This is the old debug logic, and will likely be removed eventually
		/// </summary>
		/// <param name="player">Reference to the player this is being run on</param>
		void DebugMove(DebugPlayer player) {
		    float dt = (currentTime - lastUpdates)/(float)TimeSpan.TicksPerSecond;
            if (player.MoveVector.Y >= 0)
                player.Velocity.Y = (maxVel.Y * player.MoveVector.Y * dt);
            if (player.MoveVector.Y <= 0)
                player.Velocity.Y = (maxVel.Y * player.MoveVector.Y * dt);

            if (player.MoveVector.X >= 0)
                player.Velocity.X = (maxVel.X * player.MoveVector.X * dt);
            if (player.MoveVector.X <= 0)
                player.Velocity.X = (maxVel.X * player.MoveVector.X * dt);
            player.Position += player.Velocity;
            player.Hitbox = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Width, player.Height);

            //animation
            if (player.MoveVector.Y == -1) {
                player.mult = 2;
            } else if (player.MoveVector.Y == 1) {
                player.mult = 1;
            }

            if (player.MoveVector.X == -1) {
                player.mult = 0;
            } else if (player.MoveVector.X == 1) {
                player.mult = 3;
            }

            if (player.MoveVector != Point.Zero) {
                timer += dt;
                if (timer > .25) {
                    timer = 0;
                    player.currentFrame += 1;
                }
            } else {
                player.currentFrame = 0;
            }

            //colition
            foreach (ServerEnemy ent in ent) {
                if (player.Hitbox.Intersects(ent.Hitbox) && player.dmgTimer > .5) {
                    player.dmgTimer = 0;
                    player.Health -= .25f;
                }
                player.dmgTimer += dt;
            }
        }

    }
}

