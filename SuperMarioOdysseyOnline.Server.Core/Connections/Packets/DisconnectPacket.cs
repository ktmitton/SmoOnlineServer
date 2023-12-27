using System.Buffers;

namespace SuperMarioOdysseyOnline.Server.Core.Connections.Packets;

public record DisconnectPacket(Guid Id, DisconnectData Data) : IPacket<DisconnectData>, IPacket
{
    public short Type => (short)PacketType.Disconnect;

    public DisconnectPacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new DisconnectData(data))
    {
    }

    public DisconnectPacket()
        : this(Guid.NewGuid(), new DisconnectData(ReadOnlySequence<byte>.Empty))
    {
    }

    IPacketData IPacket<IPacketData>.Data => Data;
}

public record DisconnectData(ReadOnlySequence<byte> Data) : IPacketData
{
    public short Size => (short)Data.Length;

    public byte[] ToByteArray() => Data.ToArray();
}
