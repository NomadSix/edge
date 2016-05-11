 using System;

using Edge.Hyperion.Engine;

using Type = Edge.NetCommon.Type;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Edge.Hyperion {
    public class Enemy : Entity {
        public long NetID;
        public Style Type;
        public Rectangle hitBox;
        public bool isActive = true;
        public Point Target;
        public Point MoveVector;
        public int currentframe;
        public int mult;

        public Enemy(long id, int x, int y, Style style)
            : base(id, x, y) {
            NetID = id;
            X = x;
            Y = y;
            Type = style;
            Width = 32;
            Height = 32;
            if (Type.type == NetCommon.Type.SlimeSmall || Type.type == NetCommon.Type.Fire) {
                Width = 16;
                Height = 16;
            }
            hitBox = new Rectangle(x, y, Width, Height);
        }

        public class Style {

            public Type type;
            public Texture2D Base;
            public Color BaseColour, DamageColor;

            public Style(Type type, Texture2D texture, Color? baseColour, Color? damageColour) {
                this.type = type;
                Base = texture;
                BaseColour = baseColour ?? Color.White;
                DamageColor = damageColour ?? Color.Red;
            }
        }
    }
}
