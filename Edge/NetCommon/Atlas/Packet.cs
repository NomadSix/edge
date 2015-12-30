// automatically generated, do not modify

namespace Edge.NetCommon.Atlas
{

using FlatBuffers;

///The base type for all data being transmitted
public sealed class Packet : Table {
  public static Packet GetRootAsPacket(ByteBuffer _bb) { return GetRootAsPacket(_bb, new Packet()); }
  public static Packet GetRootAsPacket(ByteBuffer _bb, Packet obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Packet __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public PacketData DataType { get { int o = __offset(4); return o != 0 ? (PacketData)bb.Get(o + bb_pos) : (PacketData)0; } }
  public TTable GetData<TTable>(TTable obj) where TTable : Table { int o = __offset(6); return o != 0 ? __union(obj, o) : null; }

  public static Offset<Packet> CreatePacket(FlatBufferBuilder builder,
      PacketData data_type = (PacketData)0,
      int data = 0) {
    builder.StartObject(2);
    Packet.AddData(builder, data);
    Packet.AddDataType(builder, data_type);
    return Packet.EndPacket(builder);
  }

  public static void StartPacket(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddDataType(FlatBufferBuilder builder, PacketData dataType) { builder.AddByte(0, (byte)(dataType), 0); }
  public static void AddData(FlatBufferBuilder builder, int dataOffset) { builder.AddOffset(1, dataOffset, 0); }
  public static Offset<Packet> EndPacket(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    builder.Required(o, 6);  // data
    return new Offset<Packet>(o);
  }
  public static void FinishPacketBuffer(FlatBufferBuilder builder, Offset<Packet> offset) { builder.Finish(offset.Value); }
};


}
