using System.Buffers;

namespace SuperMarioOdysseyOnline.Server.Packets.Data;

public record UnhandledData(ReadOnlySequence<byte> Data) : IPacketData
{
    public PacketType Type => PacketType.Unknown;

    public ReadOnlySequence<byte> AsSequence() => Data;
}
