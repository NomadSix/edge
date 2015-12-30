// automatically generated, do not modify

namespace Edge.NetCommon.Atlas
{

using FlatBuffers;

///A buff (or debuff) that gets applied to an entity
public sealed class Buff : Table {
  public static Buff GetRootAsBuff(ByteBuffer _bb) { return GetRootAsBuff(_bb, new Buff()); }
  public static Buff GetRootAsBuff(ByteBuffer _bb, Buff obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Buff __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  ///The buff being applied
  public Buffs Id { get { int o = __offset(4); return o != 0 ? (Buffs)bb.GetUshort(o + bb_pos) : (Buffs)0; } }
  ///The amount of time left until the buff runs out.
  ///-1 for permanant buffs
  public float Duration { get { int o = __offset(6); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0; } }

  public static Offset<Buff> CreateBuff(FlatBufferBuilder builder,
      Buffs id = (Buffs)0,
      float duration = 0) {
    builder.StartObject(2);
    Buff.AddDuration(builder, duration);
    Buff.AddId(builder, id);
    return Buff.EndBuff(builder);
  }

  public static void StartBuff(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddId(FlatBufferBuilder builder, Buffs id) { builder.AddUshort(0, (ushort)(id), 0); }
  public static void AddDuration(FlatBufferBuilder builder, float duration) { builder.AddFloat(1, duration, 0); }
  public static Offset<Buff> EndBuff(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Buff>(o);
  }
};


}
