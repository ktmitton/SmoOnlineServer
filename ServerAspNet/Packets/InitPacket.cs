using System.Buffers;
using SuperMarioOdysseyOnline.Server.Extensions;

namespace SuperMarioOdysseyOnline.Server.Packets;

public record InitPacket(Guid Id, InitData Data) : IPacket<InitData>, IPacket
{
    public PacketType Type => PacketType.Init;

    public InitPacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new InitData(data))
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
