// automatically generated, do not modify

namespace Edge.NetCommon.Atlas
{

using FlatBuffers;

///Requests the server to move the player
public sealed class MoveEvent : Table {
  public static MoveEvent GetRootAsMoveEvent(ByteBuffer _bb) { return GetRootAsMoveEvent(_bb, new MoveEvent()); }
  public static MoveEvent GetRootAsMoveEvent(ByteBuffer _bb, MoveEvent obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public MoveEvent __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  ///The X and Y amounts being pushed by the controller
  public Vector2 Delta { get { return GetDelta(new Vector2()); } }
  public Vector2 GetDelta(Vector2 obj) { int o = __offset(4); return o != 0 ? obj.__init(__indirect(o + bb_pos), bb) : null; }

  public static Offset<MoveEvent> CreateMoveEvent(FlatBufferBuilder builder,
      Offset<Vector2> delta = default(Offset<Vector2>)) {
    builder.StartObject(1);
    MoveEvent.AddDelta(builder, delta);
    return MoveEvent.EndMoveEvent(builder);
  }

  public static void StartMoveEvent(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddDelta(FlatBufferBuilder builder, Offset<Vector2> deltaOffset) { builder.AddOffset(0, deltaOffset.Value, 0); }
  public static Offset<MoveEvent> EndMoveEvent(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 4);  // delta
    return new Offset<MoveEvent>(o);
  }
};


}
