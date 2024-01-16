﻿using System.Buffers;
using System.Text;
using SuperMarioOdysseyOnline.Server.Extensions;
using SuperMarioOdysseyOnline.Server.Lobbies;
using SuperMarioOdysseyOnline.Server.Models;

namespace SuperMarioOdysseyOnline.Server.Connections.Packets;

public record CappyRenderPacket(Guid Id, CappyRenderData Data) : IPacket<CappyRenderData>, IPacket
{
    public short Type => (short)PacketType.CappyRenderData;

    public CappyRenderPacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new CappyRenderData(data))
    {
    }

    public CappyRenderPacket(IPlayer player)
        : this(player.Id, new CappyRenderData(player))
    {
    }

    IPacketData IPacket<IPacketData>.Data => Data;
}

public record CappyRenderData(Location Location, Animation Animation, bool IsThrown) : IPacketData
{
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

    private const short AnimationNameSize = 48;

    public short Size => sizeof(float) * 7 + sizeof(bool) + 3 + AnimationNameSize;

    public byte[] ToByteArray()
        => [
            ..BitConverter.GetBytes(Location.Position.X),
            ..BitConverter.GetBytes(Location.Position.Y),
            ..BitConverter.GetBytes(Location.Position.Z),

            ..BitConverter.GetBytes(Location.Rotation.X),
            ..BitConverter.GetBytes(Location.Rotation.Y),
            ..BitConverter.GetBytes(Location.Rotation.Z),
            ..BitConverter.GetBytes(Location.Rotation.W),

            ..BitConverter.GetBytes(IsThrown),
            0, 0, 0, // Buffer because the old packet has a size of 4 for the boolean?

            ..Encoding.UTF8.GetBytes(Animation.Name.PadRight(AnimationNameSize, '\0')),
        ];
}
