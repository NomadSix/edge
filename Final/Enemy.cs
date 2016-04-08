using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Edge.Hyperion.Engine;

namespace Edge.Hyperion {
    public class Enemy : Entity {
        public long NetID;
        public Style Type;
        public Rectangle hitBox;
        public bool isActive = true;
        public Point Target;
        public Point MoveVector;
        public int currentframe = 0;
        public int mult = 0;

        public Enemy(long id, int x, int y, Style style)
            : base(id, x, y) {
            NetID = id;
            X = x;
            Y = y;
            Width = 32;
            Height = 32;
            hitBox = new Rectangle(x, y, Width, Height);
            Type = style;
        }

        public class Style {
            public enum Type : int {
                Mage,
                Minion,
                Debug
            }

            public Texture2D Base;
            public Color BaseColour, DamageColor;

            public Style(Texture2D texture, Color? baseColour, Color? damageColour) {
                Base = texture;
                BaseColour = baseColour ?? Color.White;
                DamageColor = damageColour ?? Color.Red;
            }
        }
    }
}
