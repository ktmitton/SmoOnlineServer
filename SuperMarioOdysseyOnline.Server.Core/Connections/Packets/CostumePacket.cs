﻿using System.Buffers;
using System.Text;
using SuperMarioOdysseyOnline.Server.Extensions;
using SuperMarioOdysseyOnline.Server.Lobbies;
using SuperMarioOdysseyOnline.Server.Models;

namespace SuperMarioOdysseyOnline.Server.Connections.Packets;

public record CostumePacket(Guid Id, CostumeData Data) : IPacket<CostumeData>, IPacket
{
    public short Type => (short)PacketType.Costume;

    public CostumePacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new CostumeData(data))
    {
    }

    public CostumePacket(IPlayer player)
        : this(player.Id, new CostumeData(player))
    {
    }

    IPacketData IPacket<IPacketData>.Data => Data;
}

public record CostumeData(Costume MariosCostume, Costume CappysCostume) : IPacketData
{
    private const byte NameSize = 32;

    public CostumeData(ReadOnlySequence<byte> data)
        : this(
            new Costume(data.ReadString(0, NameSize)),
            new Costume(data.ReadString(NameSize))
        )
    {
    }

    public CostumeData(IPlayer player)
        : this(player.Mario.Costume, player.Cappy.Costume)
    {
    }

    public short Size => NameSize * 2;

    public byte[] ToByteArray()
        => [
            ..Encoding.UTF8.GetBytes(MariosCostume.Name.PadRight(NameSize, '\0')),
            ..Encoding.UTF8.GetBytes(CappysCostume.Name.PadRight(NameSize, '\0')),
        ];
}
