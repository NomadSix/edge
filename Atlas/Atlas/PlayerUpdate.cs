using System;
using System.Linq;
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
            float dt = (currentTime - lastUpdates) / (float)TimeSpan.TicksPerSecond;
            if (player.MoveVector.Y >= 0)
                player.Velocity.Y = (maxVel.Y * player.MoveVector.Y * dt);
            if (player.MoveVector.Y <= 0)
                player.Velocity.Y = (maxVel.Y * player.MoveVector.Y * dt);

            if (player.MoveVector.X >= 0)
                player.Velocity.X = (maxVel.X * player.MoveVector.X * dt);
            if (player.MoveVector.X <= 0)
                player.Velocity.X = (maxVel.X * player.MoveVector.X * dt);
            player.Position += player.Velocity;
            var hitbox = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Width, player.Height);
            player.Hitbox = hitbox;
            if (player.isAttacking) {
                if (player.mult == 0)
                    player.Atkbox = new Rectangle(hitbox.X - hitbox.Width, hitbox.Y, hitbox.Width, hitbox.Height);
                if (player.mult == 1)
                    player.Atkbox =  new Rectangle(hitbox.X, hitbox.Y + hitbox.Width, hitbox.Width, hitbox.Height);
                if (player.mult == 2)
                    player.Atkbox = new Rectangle(hitbox.X, hitbox.Y - hitbox.Width, hitbox.Width, hitbox.Height);
                if (player.mult == 3)
                    player.Atkbox = new Rectangle(hitbox.X + hitbox.Width, hitbox.Y, hitbox.Width, hitbox.Height);
            } else {
                player.Atkbox = new Rectangle();
            }

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
                player.animationTimer += dt;
                if (player.animationTimer > .25) {
                    player.animationTimer = 0;
                    player.currentFrame += 1;
                }
            } else {
                player.currentFrame = 0;
            }

            //colition
            for (int i = 0; i < ent.Count; i++) {
                switch (ent[i].entType) {
                    case ServerEnemy.Type.Debug: {
                            if (player.Hitbox.Intersects(ent[i].Hitbox) && player.dmgTimer > .5) {
                                player.dmgTimer = 0;
                                player.Health -= .125f;
                                player.isDamaged = true;
                            }
                            player.dmgTimer += dt;
                        }
                        break;
                    case ServerEnemy.Type.Mage: {
                        }
                        break;
                    case ServerEnemy.Type.Minion: {
                            if (player.Hitbox.Intersects(ent[i].Hitbox) && player.dmgTimer > .5) {
                                removeEnemys.Add(ent[i]);
                                player.dmgTimer = 0;
                                player.Health -= .25f;
                                player.isDamaged = true;
                            }
                            player.dmgTimer += dt;
                        }
                        break;
                }
                if (player.Atkbox.Intersects(ent[i].Hitbox)) {
                    ent[i].Health -= 1f;
                }
            }

            foreach (Item i in items) { 
            var q = GetQ(i);
                if (q.Count() != 0) {
                    if (player.Hitbox.Intersects(i.Hitbox))
                        player.Health = 2f;
                    }
                }

                if (player.Health <= 0) {
                    addPlayers.Add(player);
                    removePlayers.Add(player.NetID);
                }
        }
        public IEnumerable<Item> GetQ(Item i) {
            return items.Where(x =>
                Vector2.Distance(
                    new Vector2(i.Position.X + i.Width / 2, i.Position.Y + i.Height / 2),
                    new Vector2(x.Position.X + x.Width / 2, x.Position.Y + x.Height / 2))
                < 1f
                );
        }
    }
}

