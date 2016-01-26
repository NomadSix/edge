using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetStore = Edge.Hyperion.Backing.AssetStore;

namespace Edge.Hyperion {
    public class DebugTower : Entity {
        private Single Damage;
        internal TowerStyle.Team Team;
        internal Rectangle HitRectangle;
        public DebugTower(Game game, Int64 id, Single x, Single y, Single damage, TowerStyle.Team team)
            : base(game, id, x, y) {
            Damage = damage;
            Team = team;
            //int tempWidth = AssetStore.TowerStyles[team].BaseTexture.Width*10;
            //HitRectangle = new Rectangle((int)x - tempWidth/2, (int)y, tempWidth, AssetStore.TowerStyles[team].BaseTexture.Height*5);
        }
        public class TowerStyle {
            public enum Team {
                Good,
                Bad
            }

            public Texture2D BaseTexture, DamagedTexture;
            public TowerStyle(Texture2D baseTexture, Texture2D damagedTexture) {
                BaseTexture = baseTexture;

                //not going to worry about his now
                DamagedTexture = baseTexture;
            }
        }
    }
}
