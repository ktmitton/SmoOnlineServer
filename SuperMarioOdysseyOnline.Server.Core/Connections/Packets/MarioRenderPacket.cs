using System.Buffers;
using SuperMarioOdysseyOnline.Server.Core.Extensions;
using SuperMarioOdysseyOnline.Server.Core.Lobby;
using SuperMarioOdysseyOnline.Server.Core.Models;

namespace SuperMarioOdysseyOnline.Server.Core.Connections.Packets;

public record MarioRenderPacket(Guid Id, MarioRenderData Data) : IPacket<MarioRenderData>, IPacket
{
    public short Type => (short)PacketType.MarioRenderData;

    public MarioRenderPacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new MarioRenderData(data))
    {
    }

    public MarioRenderPacket(IPlayer player)
        : this(Guid.NewGuid(), new MarioRenderData(player))
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

    public short Size => (short)(
        (sizeof(float) * 7) +
        (sizeof(short) * 2) +
        (sizeof(float) * (Animation.CurrentKeyFrame?.AnimationBlendWeights.Length ?? 0))
    );

    public byte[] ToByteArray()
        => [
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
        ];
}
