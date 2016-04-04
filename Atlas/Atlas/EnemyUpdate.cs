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
            MoveTo(enemy, 100);
        }

        void MoveTo(ServerEnemy ent, float movespeed) {
            float dt = (currentTime - lastUpdates) / (float)TimeSpan.TicksPerSecond;
            //if (ent.Position == ent.MovingTo)
            //    ent.MovingTo = new Point(-1, -1);
            //if (ent.MovingTo == new Point(-1, -1)) return;
            //var deltaY = ent.MovingTo.Y - ent.Position.Y;
            //var deltaX2 = ent.MovingTo.X - ent.Position.X ^ 2;
            //var deltaY2 = deltaY ^ 2;
            //var deltaLen = (float)Math.Sqrt(deltaX2 + deltaY2);
            ////Simplified version of cos(arctan(a/b))float y = 0;
            //float y = (float)(Math.Sign(deltaY) * (movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond > deltaLen ? deltaLen : movespeed * (currentTime - lastTime) / TimeSpan.TicksPerSecond) / Math.Sqrt(1 + (deltaX2 / deltaY2)));
            ////Simplified version of sin(arctan(a/b))
            //float x = (ent.MovingTo.X - ent.Position.X) * y / (deltaY == 0 ? 1 : deltaY);
            //ent.Position += new Point((int)x, (int)y);
            var range = 32 * 7f;
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
                if (ent.Position.X < closePlayer.Position.X) { ent.Position.X += movespeed * dt; }
                if (ent.Position.Y < closePlayer.Position.Y) { ent.Position.Y += movespeed * dt; }
                if (ent.Position.X > closePlayer.Position.X) { ent.Position.X -= movespeed * dt; }
                if (ent.Position.Y > closePlayer.Position.Y) { ent.Position.Y -= movespeed * dt; }
                ent.Hitbox = new Rectangle((int)ent.Position.X, (int)ent.Position.Y, ent.Width, ent.Height);
            }
        }
    }
}
