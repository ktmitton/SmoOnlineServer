using System.Buffers;

namespace SuperMarioOdysseyOnline.Server.Packets;

public record DisconnectPacket(Guid Id, DisconnectData Data) : IPacket<DisconnectData>, IPacket
{
    public PacketType Type => PacketType.Disconnect;

    public DisconnectPacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new DisconnectData(data))
    {
    }

    IPacketData IPacket<IPacketData>.Data => Data;
}

public record DisconnectData(ReadOnlySequence<byte> Data) : IPacketData
{
    public ReadOnlySequence<byte> AsSequence() => Data;
}
