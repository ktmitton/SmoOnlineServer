using System.Buffers;

namespace SuperMarioOdysseyOnline.Server.Packets.Data;

public record DisconnectData(ReadOnlySequence<byte> Data) : IPacketData
{
    public PacketType Type => PacketType.Disconnect;

    public ReadOnlySequence<byte> AsSequence() => Data;
}
