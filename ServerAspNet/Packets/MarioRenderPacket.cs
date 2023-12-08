using System.Buffers;
using SuperMarioOdysseyOnline.Server.Extensions;
using SuperMarioOdysseyOnline.Server.Lobby;
using SuperMarioOdysseyOnline.Server.Models;

namespace SuperMarioOdysseyOnline.Server.Packets;

public record MarioRenderPacket(Guid Id, MarioRenderData Data) : IPacket<MarioRenderData>, IPacket
{
    public PacketType Type => PacketType.MarioRenderData;

    public MarioRenderPacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new MarioRenderData(data))
    {
    }

    IPacketData IPacket<IPacketData>.Data => Data;
}

public record MarioRenderData(Location Location, Animation Animation) : IPacketData
{
    public MarioRenderData(ReadOnlySequence<byte> data)
        : this(
            new Location(
                data.ReadVector3(0),
                data.ReadQuaternion(12)
            ),
            new Animation(
                new AnimationKeyFrame(
                    data.ReadUInt16(52),
                    data.ReadUInt16(54),
                    data.ReadSingleArray(28, 6)
                )
            )
        )
    {
    }

    public MarioRenderData(IPlayer player)
        : this(
            player.Mario.Location,
            player.Mario.Animation
        )
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

            ..(Animation.CurrentKeyFrame?.AnimationBlendWeights ?? []).SelectMany(x => BitConverter.GetBytes(x)),

            ..BitConverter.GetBytes(Animation.CurrentKeyFrame?.Act ?? default),
            ..BitConverter.GetBytes(Animation.CurrentKeyFrame?.SubAct ?? default)
        ]);
}
