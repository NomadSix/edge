// automatically generated, do not modify

namespace Edge.NetCommon.Atlas
{

using FlatBuffers;

///Requests the server to fire an Ability
public sealed class AbilityEvent : Table {
  public static AbilityEvent GetRootAsAbilityEvent(ByteBuffer _bb) { return GetRootAsAbilityEvent(_bb, new AbilityEvent()); }
  public static AbilityEvent GetRootAsAbilityEvent(ByteBuffer _bb, AbilityEvent obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public AbilityEvent __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  ///Ability to be fired (like, QWERDF slot)
  public AbilitySlots Id { get { int o = __offset(4); return o != 0 ? (AbilitySlots)bb.GetSbyte(o + bb_pos) : (AbilitySlots)0; } }
  public AbilityTarget TargetType { get { int o = __offset(6); return o != 0 ? (AbilityTarget)bb.Get(o + bb_pos) : (AbilityTarget)0; } }
  ///Target of the Ability (can use either targeting reguardless of the ability's targeting)
  ///Targeted abilities will use the closest Player to a point when given a Vector2
  ///Point targeted abilities will target the location of a player when given an EntityReference
  public TTable GetTarget<TTable>(TTable obj) where TTable : Table { int o = __offset(8); return o != 0 ? __union(obj, o) : null; }

  public static Offset<AbilityEvent> CreateAbilityEvent(FlatBufferBuilder builder,
      AbilitySlots id = (AbilitySlots)0,
      AbilityTarget target_type = (AbilityTarget)0,
      int target = 0) {
    builder.StartObject(3);
    AbilityEvent.AddTarget(builder, target);
    AbilityEvent.AddTargetType(builder, target_type);
    AbilityEvent.AddId(builder, id);
    return AbilityEvent.EndAbilityEvent(builder);
  }

  public static void StartAbilityEvent(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddId(FlatBufferBuilder builder, AbilitySlots id) { builder.AddSbyte(0, (sbyte)(id), 0); }
  public static void AddTargetType(FlatBufferBuilder builder, AbilityTarget targetType) { builder.AddByte(1, (byte)(targetType), 0); }
  public static void AddTarget(FlatBufferBuilder builder, int targetOffset) { builder.AddOffset(2, targetOffset, 0); }
  public static Offset<AbilityEvent> EndAbilityEvent(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 8);  // target
    return new Offset<AbilityEvent>(o);
  }
};


}
