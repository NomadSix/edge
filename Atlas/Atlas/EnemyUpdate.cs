using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Edge.Atlas {
    public partial class Atlas {

        List<DebugPlayer> Players;

        void EnemyUpdate(ServerEnemy enemy, List<DebugPlayer> players) {
            Players = players;
            //update
            Update(enemy, 60);
        }

        void Update(ServerEnemy ent, float movespeed) {
            float dt = (currentTime - lastUpdates) / (float)TimeSpan.TicksPerSecond;
            DebugPlayer closePlayer = null;
            var q = GetQ(ent);
            if (q.Count() != 0) {
                closePlayer = q.First();
                foreach (DebugPlayer player in q) {
                    if (Vector2.Distance(
                        new Vector2(ent.Position.X + ent.Width / 2, ent.Position.Y + ent.Height / 2),
                        new Vector2(player.Position.X + player.Width / 2, player.Position.Y + player.Height / 2))
                        < Vector2.Distance(
                        new Vector2(ent.Position.X + ent.Width / 2, ent.Position.Y + ent.Height / 2),
                        new Vector2(closePlayer.Position.X + closePlayer.Width / 2, closePlayer.Position.Y + closePlayer.Height / 2))
                        ) {
                        closePlayer = player;
                    }
                }

                //Update
                switch (ent.entType) {
                    case ServerEnemy.Type.Mage: {
                            var rng = new Random();
                            if (ent.summonTimer >= 1) {
                                ent.summonTimer = 0;
                                addEnemys.Add(new ServerEnemy(0, ent.Hitbox.X, ent.Hitbox.Y, ServerEnemy.Type.Minion));
                            }
                            ent.summonTimer += dt;
                        }
                        break;
                    default: {
                        }
                        break;
                }

                //move
                switch (ent.entType) {
                    case ServerEnemy.Type.Mage: {
                            movespeed = 30;
                            ent.Range = 32 * 4;
                            if (ent.Position.X + ent.Width / 4 < closePlayer.Position.X) { ent.Position.X -= movespeed * dt; }
                            if (ent.Position.Y + ent.Health / 4 < closePlayer.Position.Y) { ent.Position.Y -= movespeed * dt; }
                            if (ent.Position.X + ent.Width / 4 > closePlayer.Position.X) { ent.Position.X += movespeed * dt; }
                            if (ent.Position.Y + ent.Health / 4 > closePlayer.Position.Y) { ent.Position.Y += movespeed * dt; }
                        } break;
                    default: {
                            movespeed = 120;
                            ent.Range = 32 * 6;
                            //ent.MovingTo = 
                            //    new Vector2(
                            //        closePlayer.Position.X - closePlayer.Width / 2,
                            //        closePlayer.Position.Y - closePlayer.Height / 2
                            //    );
                            //MoveTo(ent, closePlayer, movespeed);
                            if (ent.Position.X + ent.Width / 4 < closePlayer.Position.X) {
                                ent.Position.X += movespeed * dt;
                                ent.MoveVector.X = 1;
                            }
                            if (ent.Position.Y + ent.Height / 4 < closePlayer.Position.Y) {
                                ent.Position.Y += movespeed * dt;
                                ent.MoveVector.Y = 1;
                            }
                            if (ent.Position.X + ent.Width / 4 > closePlayer.Position.X) {
                                ent.Position.X -= movespeed * dt;
                                ent.MoveVector.X = -1;
                            }
                            if (ent.Position.Y + ent.Height / 4 > closePlayer.Position.Y) {
                                ent.Position.Y -= movespeed * dt;
                                ent.MoveVector.Y = 1;
                            }

                            //animation
                            if (ent.MoveVector != Vector2.Zero) {
                                timer += dt;
                                if (timer > .25) {
                                    timer = 0;
                                    ent.currentFrame += 1;
                                }
                            } else {
                                ent.currentFrame = 0;
                            }
                            if (ent.MoveVector.Y == -1) {
                                ent.mult = 2;
                            } else if (ent.MoveVector.Y == 1) {
                                ent.mult = 0;
                            }

                            if (ent.MoveVector.X == -1) {
                                ent.mult = 1;
                            } else if (ent.MoveVector.X == 1) {
                                ent.mult = 3;
                            } break;
                }
                }
            }

            //update hitbox
            ent.Hitbox = new Rectangle((int)ent.Position.X, (int)ent.Position.Y, ent.Width, ent.Height);
        }
        public IEnumerable<DebugPlayer> GetQ(ServerEnemy ent) {
            return Players.Where(x =>
                Vector2.Distance(
                    new Vector2(ent.Position.X + ent.Width / 2, ent.Position.Y + ent.Height / 2),
                    new Vector2(x.Position.X + x.Width / 2, x.Position.Y + x.Height / 2))
                < ent.Range
                );
        }
        void MoveTo(ServerEnemy ent, DebugPlayer player, float movespeed) {
            if (ent.Position == ent.MovingTo)
                ent.MovingTo = new Vector2(-1, -1);
            if (ent.MovingTo == new Vector2(-1, -1)) return;
            var deltaY = ent.MovingTo.Y - ent.Position.Y;
            var deltaX2 = Math.Pow(ent.MovingTo.X - ent.Position.X, 2);
            var deltaY2 = Math.Pow(deltaY, 2);
            var deltaLen = (float)Math.Sqrt(deltaX2 + deltaY2);
            //Simplified version of cos(arctan(a/b))float y = 0;
            float y = (float)(Math.Sign(deltaY) * (movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond > deltaLen ? deltaLen : movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond) / Math.Sqrt(1 + (deltaX2 / deltaY2)));
            //Simplified version of sin(arctan(a/b))
            float x = (ent.MovingTo.X - ent.Position.X) * y / (deltaY == 0 ? 1 : deltaY);
            ent.Position += new Vector2(x, y);
        }
    }
}
