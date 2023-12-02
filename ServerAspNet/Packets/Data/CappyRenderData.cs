using System.Buffers;
using System.Text;
using SuperMarioOdysseyOnline.Server.Extensions;
using SuperMarioOdysseyOnline.Server.Models;
using SuperMarioOdysseyOnline.Server.Players;

namespace SuperMarioOdysseyOnline.Server.Packets.Data;

public record CappyRenderData(Location Location, Animation Animation, bool IsThrown) : IPacketData
{
    public PacketType Type => PacketType.CappyRenderData;

    public CappyRenderData(ReadOnlySequence<byte> data)
        : this(
            new Location(data.ReadVector3(), data.ReadQuaternion(12)),
            new Animation(data.ReadString(32)),
            data.ReadBoolean(28)
        )
    {
    }

    public CappyRenderData(IPlayer player)
        : this(player.Cappy.Location, player.Cappy.Animation, player.Cappy.IsThrown)
    {
    }

    public ReadOnlySequence<byte> AsSequence()
        => new([
            ..BitConverter.GetBytes(Location.Position.X),
            ..BitConverter.GetBytes(Location.Position.Y),
            ..BitConverter.GetBytes(Location.Position.Z),

            ..BitConverter.GetBytes(Location.Rotation.X),
            ..BitConverter.GetBytes(Location.Rotation.Y),
            ..BitConverter.GetBytes(Location.Rotation.Z),
            ..BitConverter.GetBytes(Location.Rotation.W),

            ..BitConverter.GetBytes(IsThrown),
            0, 0, 0, // Buffer because the old packet has a size of 4 for the boolean?

            ..Encoding.UTF8.GetBytes(Animation.Name)
        ]);
}
