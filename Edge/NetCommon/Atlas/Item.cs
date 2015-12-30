// automatically generated, do not modify

namespace Edge.NetCommon.Atlas
{

using FlatBuffers;

///An item (to modify stats outside champion level)
public sealed class Item : Table {
  public static Item GetRootAsItem(ByteBuffer _bb) { return GetRootAsItem(_bb, new Item()); }
  public static Item GetRootAsItem(ByteBuffer _bb, Item obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Item __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  ///Item ID
  public Items Id { get { int o = __offset(4); return o != 0 ? (Items)bb.GetUshort(o + bb_pos) : (Items)0; } }
  ///Count of the item (for stackables)
  public sbyte Count { get { int o = __offset(6); return o != 0 ? bb.GetSbyte(o + bb_pos) : (sbyte)0; } }
  ///Optionally, cooldown remaining for the activatable portion of an item
  public float Cooldown { get { int o = __offset(8); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0; } }

  public static Offset<Item> CreateItem(FlatBufferBuilder builder,
      Items id = (Items)0,
      sbyte count = 0,
      float cooldown = 0) {
    builder.StartObject(3);
    Item.AddCooldown(builder, cooldown);
    Item.AddId(builder, id);
    Item.AddCount(builder, count);
    return Item.EndItem(builder);
  }

  public static void StartItem(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddId(FlatBufferBuilder builder, Items id) { builder.AddUshort(0, (ushort)(id), 0); }
  public static void AddCount(FlatBufferBuilder builder, sbyte count) { builder.AddSbyte(1, count, 0); }
  public static void AddCooldown(FlatBufferBuilder builder, float cooldown) { builder.AddFloat(2, cooldown, 0); }
  public static Offset<Item> EndItem(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Item>(o);
  }
};


}
