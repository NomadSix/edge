﻿using Microsoft.Xna.Framework;

namespace Edge.Atlas {
    public class ServerEnemy : Entity {
        public long NetID;
        public Point Target;
        public Color pColor;
        public Rectangle Hitbox;
        public Type entType;
        public float summonTimer;
        public float animationTimer;
        public float lifeTimer;
        public float Range = 32*1000;
        public int currentFrame;
        public int mult;
        public bool isAttacking;
        public bool isDamaged;

        public ServerEnemy(long id, int x, int y, Type etype) : base(id, x, y) {
            Hitbox = new Rectangle(x, y, Width, Height);
            Health = 1;
            Width = 32;
            Height = 32;
            entType = etype;
            world = World.overworld;
            if (entType == Type.Mage) {
                Health = 2f;
            }
            else if (entType == Type.Minion) {
                Health = 1f;
            }
        }

        public enum Type {
            Mage,
            Minion,
            Debug,
            FireMage,
        }
    }
}