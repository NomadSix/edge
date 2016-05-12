using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;
using System.Collections.Generic;
using System.Linq;
using System;

using Type = Edge.NetCommon.Type;

namespace Edge.Atlas {
    public partial class Atlas {

        List<DebugPlayer> Players;

        void EnemyUpdate(ServerEnemy enemy) {
            Players = players.Values.ToList();
            //update
            Update(enemy, 60);
        }

        void Update(ServerEnemy ent, float movespeed) {
            float dt = (currentTime - lastUpdates) / (float)TimeSpan.TicksPerSecond;
            DebugPlayer closePlayer = null;
            var q = GetQ(ent);
            ent.stadningTimer += dt;
            if (q.Count() != 0) {
                closePlayer = q.First();
                if (closePlayer.lifeTimer > 5) {
                    for (int i = 0; i < players.Count - 1; i++) {
                        var player = q[i];
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
                    if (ent.world == closePlayer.world) {
                        Console.WriteLine(ent.entType);
                        switch (ent.entType) {
                            case Type.Mage: {
                                    var rng = new Random();
                                    if (ent.summonTimer >= .5) {
                                        ent.summonTimer = 0;
                                        addEnemys.Add(new ServerEnemy(enemys.Count + 1, ent.Hitbox.X, ent.Hitbox.Y, Type.Minion));
                                    }
                                    ent.summonTimer += dt;
                                }
                                break;
                            case Type.FireMage: {
                                    var rng = new Random();
                                    if (ent.summonTimer >= .225) {
                                        ent.summonTimer = 0;
                                        addEnemys.Add(new ServerEnemy(enemys.Count + 1, ent.Hitbox.X, ent.Hitbox.Y, Type.Fire));
                                    }
                                    ent.summonTimer += dt;
                                }
                                break;
                        }
                        //move
                        switch (ent.entType)
                        {
                            case Type.FireMage:
                            case Type.Mage: {
                                    movespeed = 30;
                                    ent.Range = 32 * 6;

                                    if (ent.Position.X + ent.Width / 4 < closePlayer.Position.X) {
                                        ent.Position.X -= movespeed * dt;
                                        ent.MoveVector.X = 1;
                                    }
                                    if (ent.Position.Y + ent.Height / 4 < closePlayer.Position.Y) {
                                        ent.Position.Y -= movespeed * dt;
                                        ent.MoveVector.Y = 1;
                                    }
                                    if (ent.Position.X + ent.Width / 4 > closePlayer.Position.X) {
                                        ent.Position.X += movespeed * dt;
                                        ent.MoveVector.X = -1;
                                    }
                                    if (ent.Position.Y + ent.Height / 4 > closePlayer.Position.Y) {
                                        ent.Position.Y += movespeed * dt;
                                        ent.MoveVector.Y = -1;
                                    }

                                    if (ent.MoveVector.X == -1) {
                                        ent.mult = 0;
                                    } else if (ent.MoveVector.X == 1) {
                                        ent.mult = 1;
                                    }
                                }
                                break;
                            case Type.Slime:
                                movespeed = 15f;
                                ent.Range = 32 * 8;
                                if (ent.moveTimer > 1) {
                                    ent.moveTimer = 0;
                                } else if (ent.moveTimer > .5) {
                                    movespeed = 15;
                                } else if (ent.moveTimer > .25) {
                                    movespeed = 90;
                                }
                                ent.moveTimer += dt;
                                if (ent.Position.X + ent.Width / 2 < closePlayer.Position.X) {
                                    ent.Position.X += movespeed * dt;
                                    ent.MoveVector.X = 1;
                                }
                                if (ent.Position.Y + ent.Height / 2 < closePlayer.Position.Y) {
                                    ent.Position.Y += movespeed * dt;
                                    ent.MoveVector.Y = 1;
                                }
                                if (ent.Position.X + ent.Width / 2 > closePlayer.Position.X) {
                                    ent.Position.X -= movespeed * dt;
                                    ent.MoveVector.X = -1;
                                }
                                if (ent.Position.Y + ent.Height / 2 > closePlayer.Position.Y) {
                                    ent.Position.Y -= movespeed * dt;
                                    ent.MoveVector.Y = -1;
                                }
                                ent.mult = 0;
                                break;
                            case Type.SlimeSmall:
                                movespeed = 15f;
                                ent.Range = 32 * 10;
                                if (ent.moveTimer > 1) {
                                    ent.moveTimer = 0;
                                } else if (ent.moveTimer > .5) {
                                    movespeed = 15;
                                } else if (ent.moveTimer > .25) {
                                    movespeed = 90;
                                }
                                ent.moveTimer += dt;
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
                                    ent.MoveVector.Y = -1;
                                }
                                ent.mult = 0;
                                break;
                            case Type.Fire:
                                movespeed = 2000;
                                ent.Range = 32 * 12;
                                ent.MovingTo =
                                    new Vector2(
                                        closePlayer.Position.X - closePlayer.Width / 2,
                                        closePlayer.Position.Y - closePlayer.Height / 2
                                    );
                                MoveTo(ent, closePlayer, movespeed);
                                break;
                            default: {
                                    movespeed = 90;
                                    ent.Range = 32 * 7;
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
                                        ent.MoveVector.Y = -1;
                                    }

                                    if (ent.MoveVector.X == -1) {
                                        ent.mult = 0;
                                    } else if (ent.MoveVector.X == 1) {
                                        ent.mult = 1;
                                    }
                                    break;
                                }
                        }

                        //animation
                        if (ent.MoveVector != Vector2.Zero) {
                            ent.animationTimer += dt;
                            if (ent.animationTimer > .15) {
                                ent.animationTimer = 0;
                                ent.currentFrame += 1;
                            }
                        } else {
                            ent.stadningTimer += dt;
                        }

                        for (int i = 0; i < walls.Length; i++) {
                            var wall = walls[i];
                            if (ent.Hitbox.Intersects(wall)) {
                                if (ent.Hitbox.Top - (maxVel.Y * ent.MoveVector.Y * dt) <= wall.Bottom && ent.Hitbox.Top >= wall.Bottom - 5)
                                    ent.Position.Y = wall.Bottom;
                                if (ent.Hitbox.Right + (maxVel.X * ent.MoveVector.X * dt) >= wall.Left && ent.Hitbox.Right <= wall.Left + 5)
                                    ent.Position.X = wall.Left - ent.Hitbox.Width;
                                if (ent.Hitbox.Left - (maxVel.X * ent.MoveVector.X * dt) <= wall.Right && ent.Hitbox.Left >= wall.Right - 5)
                                    ent.Position.X = wall.Right;
                                if (ent.Hitbox.Bottom + (maxVel.Y * ent.MoveVector.Y * dt) >= wall.Top && ent.Hitbox.Bottom <= wall.Top + 5)
                                    ent.Position.Y = wall.Top - ent.Hitbox.Height;
                            }
                        }

                        if (ent.Health < 0) { 
                            die(ent);
                        }

                    } else {
                        ent.currentFrame = 0;
                    }
                }
                //update hitbox
                ent.Hitbox = new Rectangle((int)ent.Position.X, (int)ent.Position.Y, ent.Width, ent.Height);
            }
            ent.lifeTimer += dt;

            //despawn
            if (ent.lifeTimer > 2 && ent.entType == Type.Minion) {
                ent.lifeTimer = 0;
                die(ent);
            }
            if (ent.lifeTimer > .5 && ent.entType == Type.Fire)
            {
                ent.lifeTimer = 0;
                die(ent);
            }
        }

        void die(ServerEnemy ent) {
            if (ent.Health <= 0 && ent.entType != Type.Fire)
                items.Add(new Item(items.Count + 1, (int)ent.Position.X + ent.Width / 4, (int)ent.Position.Y + ent.Height / 4, (Item.Type)new Random().Next(2)));
            if (ent.entType == Type.Slime) {
                addEnemys.Add(new ServerEnemy(enemys.Count + 1, (int)ent.Position.X + ent.Width / 2, (int)ent.Position.Y + ent.Height / 2, Type.SlimeSmall));
                addEnemys.Add(new ServerEnemy(enemys.Count + 1, (int)ent.Position.X - ent.Width / 2, (int)ent.Position.Y - ent.Height / 2, Type.SlimeSmall));
            }
            removeEnemys.Add(ent);
        }

        public List<DebugPlayer> GetQ(ServerEnemy ent) {
            var results = new List<DebugPlayer>();
            for (int i = 0; i < Players.Count; i++) {
                var x = Players[i];
                if (Vector2.Distance(
                    new Vector2(ent.Position.X + ent.Width / 2, ent.Position.Y + ent.Height / 2),
                    new Vector2(x.Position.X + x.Width / 2, x.Position.Y + x.Height / 2))
                < ent.Range) {
                    results.Add(x);
                }
            }
            return results;
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
        void MoveToPoint(ServerEnemy ent, Vector2 player, float movespeed)
        {
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
