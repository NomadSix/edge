// automatically generated, do not modify

namespace Edge.NetCommon.Atlas
{

using FlatBuffers;

///The main thing the server is going to send
///Contains entity changes (or a keyframe with all current data)
///And a the projectiles this player knows about
public sealed class EntityPulse : Table {
  public static EntityPulse GetRootAsEntityPulse(ByteBuffer _bb) { return GetRootAsEntityPulse(_bb, new EntityPulse()); }
  public static EntityPulse GetRootAsEntityPulse(ByteBuffer _bb, EntityPulse obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public EntityPulse __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  ///[Changed] entities the player knows about
  public Entity GetEntities(int j) { return GetEntities(new Entity(), j); }
  public Entity GetEntities(Entity obj, int j) { int o = __offset(4); return o != 0 ? obj.__init(__indirect(__vector(o) + j * 4), bb) : null; }
  public int EntitiesLength { get { int o = __offset(4); return o != 0 ? __vector_len(o) : 0; } }
  ///[Changed] ability results that the player can see
  public Projectile GetProjectiles(int j) { return GetProjectiles(new Projectile(), j); }
  public Projectile GetProjectiles(Projectile obj, int j) { int o = __offset(6); return o != 0 ? obj.__init(__indirect(__vector(o) + j * 4), bb) : null; }
  public int ProjectilesLength { get { int o = __offset(6); return o != 0 ? __vector_len(o) : 0; } }

  public static Offset<EntityPulse> CreateEntityPulse(FlatBufferBuilder builder,
      VectorOffset entities = default(VectorOffset),
      VectorOffset projectiles = default(VectorOffset)) {
    builder.StartObject(2);
    EntityPulse.AddProjectiles(builder, projectiles);
    EntityPulse.AddEntities(builder, entities);
    return EntityPulse.EndEntityPulse(builder);
  }

  public static void StartEntityPulse(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddEntities(FlatBufferBuilder builder, VectorOffset entitiesOffset) { builder.AddOffset(0, entitiesOffset.Value, 0); }
  public static VectorOffset CreateEntitiesVector(FlatBufferBuilder builder, Offset<Entity>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartEntitiesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddProjectiles(FlatBufferBuilder builder, VectorOffset projectilesOffset) { builder.AddOffset(1, projectilesOffset.Value, 0); }
  public static VectorOffset CreateProjectilesVector(FlatBufferBuilder builder, Offset<Projectile>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartProjectilesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<EntityPulse> EndEntityPulse(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<EntityPulse>(o);
  }
};


}
