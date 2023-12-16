using System.Buffers;
using SuperMarioOdysseyOnline.Server.Core.Extensions;

namespace SuperMarioOdysseyOnline.Server.Core.Connections.Packets;

public record InitPacket(Guid Id, InitData Data) : IPacket<InitData>, IPacket
{
    public short Type => (short)PacketType.Init;

    public InitPacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new InitData(data))
    {
    }

    public InitPacket(ushort maxPlayers)
        : this(Guid.NewGuid(), new InitData(maxPlayers))
    {
    }

    IPacketData IPacket<IPacketData>.Data => Data;
}

public record InitData(ushort MaxPlayers) : IPacketData
{
    public InitData(ReadOnlySequence<byte> data)
        : this(data.ReadUInt16(0))
    {
    }

    public short Size => sizeof(short);

    public byte[] ToByteArray()
        => BitConverter.GetBytes(MaxPlayers);
}
