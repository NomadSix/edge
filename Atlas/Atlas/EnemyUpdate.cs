using Microsoft.Xna.Framework;
using System;

namespace Edge.Atlas {
    public partial class Atlas {

        void EnemyUpdate(ServerEnemy enemy) {
            //update
            enemy.MovingTo = new Vector2(100, 100);
            MoveTo(enemy, 5);
        }
        
        void MoveTo(ServerEnemy ent, float movespeed) {
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
