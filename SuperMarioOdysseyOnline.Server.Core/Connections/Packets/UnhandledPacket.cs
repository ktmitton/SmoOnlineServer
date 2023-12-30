using System.Buffers;

namespace SuperMarioOdysseyOnline.Server.Connections.Packets;

public record UnhandledPacket(Guid Id, UnhandledData Data) : IPacket<UnhandledData>, IPacket
{
    public short Type => (short)PacketType.Unknown;

    public UnhandledPacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new UnhandledData(data))
    {
    }

    IPacketData IPacket<IPacketData>.Data => Data;
}

public record UnhandledData(ReadOnlySequence<byte> Data) : IPacketData
{
    public short Size => (short)Data.Length;

    public byte[] ToByteArray() => Data.ToArray();
}
