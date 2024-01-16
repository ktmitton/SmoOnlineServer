using System.Buffers;
using SuperMarioOdysseyOnline.Server.Extensions;
using SuperMarioOdysseyOnline.Server.Lobbies;
using SuperMarioOdysseyOnline.Server.Models;

namespace SuperMarioOdysseyOnline.Server.Connections.Packets;

public record MarioRenderPacket(Guid Id, MarioRenderData Data) : IPacket<MarioRenderData>, IPacket
{
    public short Type => (short)PacketType.MarioRenderData;

    public MarioRenderPacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new MarioRenderData(data))
    {
    }

    public MarioRenderPacket(IPlayer player)
        : this(player.Id, new MarioRenderData(player))
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

    public short Size => (sizeof(float) * 7) + (sizeof(short) * 2) + (sizeof(float) * 6);

    public byte[] ToByteArray()
        => [
            ..BitConverter.GetBytes(Location.Position.X),
            ..BitConverter.GetBytes(Location.Position.Y),
            ..BitConverter.GetBytes(Location.Position.Z),

            ..BitConverter.GetBytes(Location.Rotation.X),
            ..BitConverter.GetBytes(Location.Rotation.Y),
            ..BitConverter.GetBytes(Location.Rotation.Z),
            ..BitConverter.GetBytes(Location.Rotation.W),

            ..BitConverter.GetBytes(Animation.CurrentKeyFrame?.AnimationBlendWeights.Weight0 ?? default),
            ..BitConverter.GetBytes(Animation.CurrentKeyFrame?.AnimationBlendWeights.Weight1 ?? default),
            ..BitConverter.GetBytes(Animation.CurrentKeyFrame?.AnimationBlendWeights.Weight2 ?? default),
            ..BitConverter.GetBytes(Animation.CurrentKeyFrame?.AnimationBlendWeights.Weight3 ?? default),
            ..BitConverter.GetBytes(Animation.CurrentKeyFrame?.AnimationBlendWeights.Weight4 ?? default),
            ..BitConverter.GetBytes(Animation.CurrentKeyFrame?.AnimationBlendWeights.Weight5 ?? default),

            ..BitConverter.GetBytes(Animation.CurrentKeyFrame?.Act ?? default),
            ..BitConverter.GetBytes(Animation.CurrentKeyFrame?.SubAct ?? default)
        ];
}
