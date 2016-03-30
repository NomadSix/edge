using Microsoft.Xna.Framework;
using System;

namespace Edge.Atlas {
    public partial class Atlas {

        void EnemyUpdate(ServerEnemy enemy, DebugPlayer player) {
            //update
            enemy.MovingTo = new Point(player.Position.X, player.Position.Y);
            MoveTo(enemy, player, 1);
        }
        
        void MoveTo(ServerEnemy ent, DebugPlayer player, int movespeed) {
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

            if (ent.Position.X < player.Position.X) { ent.Position.X += movespeed; }
            if (ent.Position.Y < player.Position.Y) { ent.Position.Y += movespeed; }
            if (ent.Position.X > player.Position.X) { ent.Position.X -= movespeed; }
            if (ent.Position.Y > player.Position.Y) { ent.Position.Y -= movespeed; }
        }
    }
}
