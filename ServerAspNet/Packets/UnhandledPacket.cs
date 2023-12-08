using System.Buffers;

namespace SuperMarioOdysseyOnline.Server.Packets;

public record UnhandledPacket(Guid Id, UnhandledData Data) : IPacket<UnhandledData>, IPacket
{
    public PacketType Type => PacketType.Unknown;

    public UnhandledPacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new UnhandledData(data))
    {
    }

    IPacketData IPacket<IPacketData>.Data => Data;
}

public record UnhandledData(ReadOnlySequence<byte> Data) : IPacketData
{
    public ReadOnlySequence<byte> AsSequence() => Data;
}
