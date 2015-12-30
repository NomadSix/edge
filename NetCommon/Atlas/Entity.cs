// automatically generated, do not modify

namespace Edge.NetCommon.Atlas
{

using FlatBuffers;

///Anything that exists in the game
///Players, Minions, turrets
public sealed class Entity : Table {
  public static Entity GetRootAsEntity(ByteBuffer _bb) { return GetRootAsEntity(_bb, new Entity()); }
  public static Entity GetRootAsEntity(ByteBuffer _bb, Entity obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Entity __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  ///The ID of an entity (the RemoteUniqueID in the case of Players)
  public long Id { get { int o = __offset(4); return o != 0 ? bb.GetLong(o + bb_pos) : (long)0; } }
  ///The model being used on this entity
  public Models Model { get { int o = __offset(6); return o != 0 ? (Models)bb.GetUshort(o + bb_pos) : (Models)0; } }
  ///The entity's Hit Points
  public uint Hp { get { int o = __offset(8); return o != 0 ? bb.GetUint(o + bb_pos) : (uint)0; } }
  ///The entity's amount of resource (mana or energy or whatever)
  public uint Resource { get { int o = __offset(10); return o != 0 ? bb.GetUint(o + bb_pos) : (uint)0; } }
  ///The level of the entity (for stats)
  public sbyte Level { get { int o = __offset(12); return o != 0 ? bb.GetSbyte(o + bb_pos) : (sbyte)0; } }
  ///The entity's current location
  public Vector2 Location { get { return GetLocation(new Vector2()); } }
  public Vector2 GetLocation(Vector2 obj) { int o = __offset(14); return o != 0 ? obj.__init(__indirect(o + bb_pos), bb) : null; }
  ///The point that the entity is moving towards (in the case of a minion), or facing (all other entities)
  public Vector2 Target { get { return GetTarget(new Vector2()); } }
  public Vector2 GetTarget(Vector2 obj) { int o = __offset(16); return o != 0 ? obj.__init(__indirect(o + bb_pos), bb) : null; }
  ///Any buffs (or debuffs) currently on the entity
  public Buff GetBuffs(int j) { return GetBuffs(new Buff(), j); }
  public Buff GetBuffs(Buff obj, int j) { int o = __offset(18); return o != 0 ? obj.__init(__indirect(__vector(o) + j * 4), bb) : null; }
  public int BuffsLength { get { int o = __offset(18); return o != 0 ? __vector_len(o) : 0; } }
  ///The state of the entity's abilities
  public AbilityStatus GetAbilities(int j) { return GetAbilities(new AbilityStatus(), j); }
  public AbilityStatus GetAbilities(AbilityStatus obj, int j) { int o = __offset(20); return o != 0 ? obj.__init(__indirect(__vector(o) + j * 4), bb) : null; }
  public int AbilitiesLength { get { int o = __offset(20); return o != 0 ? __vector_len(o) : 0; } }
  ///The entity's inventory
  public Item GetInventory(int j) { return GetInventory(new Item(), j); }
  public Item GetInventory(Item obj, int j) { int o = __offset(22); return o != 0 ? obj.__init(__indirect(__vector(o) + j * 4), bb) : null; }
  public int InventoryLength { get { int o = __offset(22); return o != 0 ? __vector_len(o) : 0; } }

  public static Offset<Entity> CreateEntity(FlatBufferBuilder builder,
      long id = 0,
      Models model = (Models)0,
      uint hp = 0,
      uint resource = 0,
      sbyte level = 0,
      Offset<Vector2> location = default(Offset<Vector2>),
      Offset<Vector2> target = default(Offset<Vector2>),
      VectorOffset buffs = default(VectorOffset),
      VectorOffset abilities = default(VectorOffset),
      VectorOffset inventory = default(VectorOffset)) {
    builder.StartObject(10);
    Entity.AddId(builder, id);
    Entity.AddInventory(builder, inventory);
    Entity.AddAbilities(builder, abilities);
    Entity.AddBuffs(builder, buffs);
    Entity.AddTarget(builder, target);
    Entity.AddLocation(builder, location);
    Entity.AddResource(builder, resource);
    Entity.AddHp(builder, hp);
    Entity.AddModel(builder, model);
    Entity.AddLevel(builder, level);
    return Entity.EndEntity(builder);
  }

  public static void StartEntity(FlatBufferBuilder builder) { builder.StartObject(10); }
  public static void AddId(FlatBufferBuilder builder, long id) { builder.AddLong(0, id, 0); }
  public static void AddModel(FlatBufferBuilder builder, Models model) { builder.AddUshort(1, (ushort)(model), 0); }
  public static void AddHp(FlatBufferBuilder builder, uint hp) { builder.AddUint(2, hp, 0); }
  public static void AddResource(FlatBufferBuilder builder, uint resource) { builder.AddUint(3, resource, 0); }
  public static void AddLevel(FlatBufferBuilder builder, sbyte level) { builder.AddSbyte(4, level, 0); }
  public static void AddLocation(FlatBufferBuilder builder, Offset<Vector2> locationOffset) { builder.AddOffset(5, locationOffset.Value, 0); }
  public static void AddTarget(FlatBufferBuilder builder, Offset<Vector2> targetOffset) { builder.AddOffset(6, targetOffset.Value, 0); }
  public static void AddBuffs(FlatBufferBuilder builder, VectorOffset buffsOffset) { builder.AddOffset(7, buffsOffset.Value, 0); }
  public static VectorOffset CreateBuffsVector(FlatBufferBuilder builder, Offset<Buff>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartBuffsVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddAbilities(FlatBufferBuilder builder, VectorOffset abilitiesOffset) { builder.AddOffset(8, abilitiesOffset.Value, 0); }
  public static VectorOffset CreateAbilitiesVector(FlatBufferBuilder builder, Offset<AbilityStatus>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartAbilitiesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddInventory(FlatBufferBuilder builder, VectorOffset inventoryOffset) { builder.AddOffset(9, inventoryOffset.Value, 0); }
  public static VectorOffset CreateInventoryVector(FlatBufferBuilder builder, Offset<Item>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartInventoryVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<Entity> EndEntity(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Entity>(o);
  }
};


}
