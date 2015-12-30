using System;
using Buffers = Edge.NetCommon.Atlas;
using FlatBuffers;
using System.Collections.Generic;

namespace Edge.Atlas.Components {
	public class Projectile {
		public Buffers.Vector2 Position;
		public UInt16 Effect;

		public Projectile() {
		}

		public Offset<Buffers.Projectile> ToBuffer(FlatBufferBuilder fbb) {
			Buffers.Projectile.CreateProjectile(fbb, Buffers.Vector2.CreateVector2(fbb, Position.X, Position.Y), Effect);
		}
	}
}

