using System.Buffers;
using SuperMarioOdysseyOnline.Server.Extensions;

namespace SuperMarioOdysseyOnline.Server.Packets;

public record ShinePacket(Guid Id, ShineData Data) : IPacket<ShineData>, IPacket
{
    public PacketType Type => PacketType.Shine;

    public ShinePacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new ShineData(data))
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
