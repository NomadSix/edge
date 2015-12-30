// automatically generated, do not modify

namespace Edge.NetCommon.Atlas
{

using FlatBuffers;

///A reference to an entity (mainly so we can put it into the union)
public sealed class EntityReference : Table {
  public static EntityReference GetRootAsEntityReference(ByteBuffer _bb) { return GetRootAsEntityReference(_bb, new EntityReference()); }
  public static EntityReference GetRootAsEntityReference(ByteBuffer _bb, EntityReference obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public EntityReference __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  ///The entity's ID
  public long Id { get { int o = __offset(4); return o != 0 ? bb.GetLong(o + bb_pos) : (long)0; } }

  public static Offset<EntityReference> CreateEntityReference(FlatBufferBuilder builder,
      long id = 0) {
    builder.StartObject(1);
    EntityReference.AddId(builder, id);
    return EntityReference.EndEntityReference(builder);
  }

  public static void StartEntityReference(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddId(FlatBufferBuilder builder, long id) { builder.AddLong(0, id, 0); }
  public static Offset<EntityReference> EndEntityReference(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<EntityReference>(o);
  }
};


}
