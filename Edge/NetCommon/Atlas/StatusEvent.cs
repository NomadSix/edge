// automatically generated, do not modify

namespace Edge.NetCommon.Atlas
{

using FlatBuffers;

///An event that's not covered by EntityPulses
///IE: Player/Turret Deaths, Disconnect/Reconnects
public sealed class StatusEvent : Table {
  public static StatusEvent GetRootAsStatusEvent(ByteBuffer _bb) { return GetRootAsStatusEvent(_bb, new StatusEvent()); }
  public static StatusEvent GetRootAsStatusEvent(ByteBuffer _bb, StatusEvent obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public StatusEvent __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public Events Id { get { int o = __offset(4); return o != 0 ? (Events)bb.GetSbyte(o + bb_pos) : (Events)0; } }
  public string Message { get { int o = __offset(6); return o != 0 ? __string(o + bb_pos) : null; } }

  public static Offset<StatusEvent> CreateStatusEvent(FlatBufferBuilder builder,
      Events id = (Events)0,
      StringOffset message = default(StringOffset)) {
    builder.StartObject(2);
    StatusEvent.AddMessage(builder, message);
    StatusEvent.AddId(builder, id);
    return StatusEvent.EndStatusEvent(builder);
  }

  public static void StartStatusEvent(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddId(FlatBufferBuilder builder, Events id) { builder.AddSbyte(0, (sbyte)(id), 0); }
  public static void AddMessage(FlatBufferBuilder builder, StringOffset messageOffset) { builder.AddOffset(1, messageOffset.Value, 0); }
  public static Offset<StatusEvent> EndStatusEvent(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<StatusEvent>(o);
  }
};


}
