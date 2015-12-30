// automatically generated, do not modify

namespace Edge.NetCommon.Atlas
{

using FlatBuffers;

///A 2 Dimentional Vector
public sealed class Vector2 : Table {
  public static Vector2 GetRootAsVector2(ByteBuffer _bb) { return GetRootAsVector2(_bb, new Vector2()); }
  public static Vector2 GetRootAsVector2(ByteBuffer _bb, Vector2 obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Vector2 __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  ///X Component of the Vector
  public float X { get { int o = __offset(4); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0; } }
  ///Y Component of the Vector
  public float Y { get { int o = __offset(6); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0; } }

  public static Offset<Vector2> CreateVector2(FlatBufferBuilder builder,
      float x = 0,
      float y = 0) {
    builder.StartObject(2);
    Vector2.AddY(builder, y);
    Vector2.AddX(builder, x);
    return Vector2.EndVector2(builder);
  }

  public static void StartVector2(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddX(FlatBufferBuilder builder, float x) { builder.AddFloat(0, x, 0); }
  public static void AddY(FlatBufferBuilder builder, float y) { builder.AddFloat(1, y, 0); }
  public static Offset<Vector2> EndVector2(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Vector2>(o);
  }
};


}
