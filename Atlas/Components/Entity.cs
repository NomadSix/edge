using System;
using Buffers = Edge.NetCommon.Atlas;
using FlatBuffers;
using System.Collections.Generic;

namespace Edge.Atlas.Components {
	public class Entity {
		public Int64 ID;
		public Buffers.Models Model;
		public UInt32 HP;
		public UInt32 Resource;
		public SByte Level;
		public Buffers.Vector2 Location;
		public Buffers.Vector2 Target;
		public List<Buffers.Buff> Buffs;
		public List<Buffers.AbilityStatus> Abilities;
		public List<Buffers.Item> Inventory;

		public Entity() {
		}

		public void Update(Int64 deltaTicks) {
			
		}

		#region FlatBuffers Convenience Stuff

		public Entity(Buffers.Entity fromBuffer) {
			ID = fromBuffer.Id;
			Model = fromBuffer.Model;
			HP = fromBuffer.Hp;
			Resource = fromBuffer.Resource;
			Level = fromBuffer.Level;
			Location = fromBuffer.Location;
			Target = fromBuffer.Target;
			Buffs = new List<Buffers.Buff>();
			for(int i = 0; i < fromBuffer.BuffsLength; i++)
				Buffs.Add(fromBuffer.GetBuffs(i));
			Abilities = new List<Buffers.AbilityStatus>();
			for(int i = 0; i < fromBuffer.AbilitiesLength; i++)
				Abilities.Add(fromBuffer.GetAbilities(i));
			Inventory = new List<Buffers.Item>();
			for(int i = 0; i < fromBuffer.InventoryLength; i++)
				Inventory.Add(fromBuffer.GetInventory(i));
		}
		//TODO: Figure out how to compute the changes
		/// <summary>
		/// Converts the entity to FlatBuffers offsets in a buffer.
		/// </summary>
		/// <returns>The generated offset</returns>
		/// <param name="fbb">The FlatBufferBuilder to work on</param>
		public Offset<Buffers.Entity> ToBuffer(FlatBufferBuilder fbb) {
			var ios = new Offset<Buffers.Item>[Inventory.Count];
			for(int j = 0, InventoryCount = Inventory.Count; j < InventoryCount; j++) {
				var i = Inventory[j];
				ios[j] = Buffers.Item.CreateItem(fbb, i.Id, i.Count, i.Cooldown);
			}
			var inventory = Buffers.Entity.CreateInventoryVector(fbb, ios);

			var aos = new Offset<Buffers.AbilityStatus>[Abilities.Count];
			for(int i = 0, AbilitiesCount = Abilities.Count; i < AbilitiesCount; i++) {
				var a = Abilities[i];
				aos[i] = Buffers.AbilityStatus.CreateAbilityStatus(fbb, a.Id, a.Cooldown);
			}
			var abilities = Buffers.Entity.CreateAbilitiesVector(fbb, aos);

			var bos = new Offset<Buffers.Buff>[Buffs.Count];
			for(int i = 0, BuffsCount = Buffs.Count; i < BuffsCount; i++) {
				var b = Buffs[i];
				bos[i] = Buffers.Buff.CreateBuff(fbb, b.Id, b.Duration);
			}
			var buffs = Buffers.Entity.CreateBuffsVector(fbb, bos);

			var location = Buffers.Vector2.CreateVector2(fbb, Location.X, Location.Y);
			var target = Buffers.Vector2.CreateVector2(fbb, Target.X, Target.Y);
			return Buffers.Entity.CreateEntity(fbb, ID, Model, HP, Resource, Level, location, target, buffs, abilities, inventory);
		}

		#endregion
	}
}

