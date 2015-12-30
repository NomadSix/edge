// automatically generated, do not modify

namespace Edge.NetCommon.Atlas
{

using FlatBuffers;

///This says projectile, but it really refers to any sort of ability result
///Line missile, or AOE effect
public sealed class Projectile : Table {
  public static Projectile GetRootAsProjectile(ByteBuffer _bb) { return GetRootAsProjectile(_bb, new Projectile()); }
  public static Projectile GetRootAsProjectile(ByteBuffer _bb, Projectile obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Projectile __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  ///Position of 'projectile'
  public Vector2 Position { get { return GetPosition(new Vector2()); } }
  public Vector2 GetPosition(Vector2 obj) { int o = __offset(4); return o != 0 ? obj.__init(__indirect(o + bb_pos), bb) : null; }
  ///Effect used to render it
  public ushort Effect { get { int o = __offset(6); return o != 0 ? bb.GetUshort(o + bb_pos) : (ushort)0; } }

  public static Offset<Projectile> CreateProjectile(FlatBufferBuilder builder,
      Offset<Vector2> position = default(Offset<Vector2>),
      ushort effect = 0) {
    builder.StartObject(2);
    Projectile.AddPosition(builder, position);
    Projectile.AddEffect(builder, effect);
    return Projectile.EndProjectile(builder);
  }

  public static void StartProjectile(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddPosition(FlatBufferBuilder builder, Offset<Vector2> positionOffset) { builder.AddOffset(0, positionOffset.Value, 0); }
  public static void AddEffect(FlatBufferBuilder builder, ushort effect) { builder.AddUshort(1, effect, 0); }
  public static Offset<Projectile> EndProjectile(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Projectile>(o);
  }
};


}
