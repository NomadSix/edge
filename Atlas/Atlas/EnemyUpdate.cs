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
            MoveTo(enemy, 60);
        }

        void MoveTo(ServerEnemy ent, float movespeed) {
            float dt = (currentTime - lastUpdates) / (float)TimeSpan.TicksPerSecond;
            var range = 32 * 7f; // how far the enemy looks for players
            DebugPlayer closePlayer = null;
            var q = Players.Where(x =>
                Vector2.Distance(
                    new Vector2(ent.Position.X + ent.Width / 2, ent.Position.Y + ent.Height / 2),
                    new Vector2(x.Position.X + x.Width / 2, x.Position.Y + x.Height / 2))
                < range
                );

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

                //move
                switch (ent.entType) {
                    case ServerEnemy.Type.Mage: {
                            if (ent.Position.X < closePlayer.Position.X) { ent.Position.X -= movespeed * dt; }
                            if (ent.Position.Y < closePlayer.Position.Y) { ent.Position.Y -= movespeed * dt; }
                            if (ent.Position.X > closePlayer.Position.X) { ent.Position.X += movespeed * dt; }
                            if (ent.Position.Y > closePlayer.Position.Y) { ent.Position.Y += movespeed * dt; }

                            //action
                            //rng number to make a minions
                        } break;
                    default: {
                            if (ent.Position.X < closePlayer.Position.X) { ent.Position.X += movespeed * dt; }
                            if (ent.Position.Y < closePlayer.Position.Y) { ent.Position.Y += movespeed * dt; }
                            if (ent.Position.X > closePlayer.Position.X) { ent.Position.X -= movespeed * dt; }
                            if (ent.Position.Y > closePlayer.Position.Y) { ent.Position.Y -= movespeed * dt; }
                        } break;
                }
                //update hitbox
                ent.Hitbox = new Rectangle((int)ent.Position.X, (int)ent.Position.Y, ent.Width, ent.Height);
            }
        }
    }
}
