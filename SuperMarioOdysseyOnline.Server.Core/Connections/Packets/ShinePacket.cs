using System.Buffers;
using SuperMarioOdysseyOnline.Server.Core.Extensions;

namespace SuperMarioOdysseyOnline.Server.Core.Connections.Packets;

public record ShinePacket(Guid Id, ShineData Data) : IPacket<ShineData>, IPacket
{
    public short Type => (short)PacketType.Shine;

    public ShinePacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new ShineData(data))
    {
    }

    public ShinePacket(int shineId) : this(Guid.NewGuid(), new ShineData(shineId))
    {
    }

    IPacketData IPacket<IPacketData>.Data => Data;
}

public record ShineData(int ShineId) : IPacketData
{
    public ShineData(ReadOnlySequence<byte> data)
        : this(data.ReadInt32(0))
    {
    }

    public short Size => sizeof(int);

    public byte[] ToByteArray()
        => BitConverter.GetBytes(ShineId);
}
