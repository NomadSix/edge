// automatically generated, do not modify

namespace Edge.NetCommon.Atlas
{

using FlatBuffers;

///Represents a player Ability's status
public sealed class AbilityStatus : Table {
  public static AbilityStatus GetRootAsAbilityStatus(ByteBuffer _bb) { return GetRootAsAbilityStatus(_bb, new AbilityStatus()); }
  public static AbilityStatus GetRootAsAbilityStatus(ByteBuffer _bb, AbilityStatus obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public AbilityStatus __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  ///The ability slot being referred to (client can determine ability details from model and slot)
  public AbilitySlots Id { get { int o = __offset(4); return o != 0 ? (AbilitySlots)bb.GetSbyte(o + bb_pos) : (AbilitySlots)0; } }
  ///The current cooldown of this ability (in seconds)
  ///-1 for when the use of the ability was not seen
  public float Cooldown { get { int o = __offset(6); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0; } }

  public static Offset<AbilityStatus> CreateAbilityStatus(FlatBufferBuilder builder,
      AbilitySlots id = (AbilitySlots)0,
      float cooldown = 0) {
    builder.StartObject(2);
    AbilityStatus.AddCooldown(builder, cooldown);
    AbilityStatus.AddId(builder, id);
    return AbilityStatus.EndAbilityStatus(builder);
  }

  public static void StartAbilityStatus(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddId(FlatBufferBuilder builder, AbilitySlots id) { builder.AddSbyte(0, (sbyte)(id), 0); }
  public static void AddCooldown(FlatBufferBuilder builder, float cooldown) { builder.AddFloat(1, cooldown, 0); }
  public static Offset<AbilityStatus> EndAbilityStatus(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<AbilityStatus>(o);
  }
};


}
