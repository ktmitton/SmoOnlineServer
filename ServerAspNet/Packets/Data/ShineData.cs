using System.Buffers;
using SuperMarioOdysseyOnline.Server.Extensions;

namespace SuperMarioOdysseyOnline.Server.Packets.Data;

public record ShineData(int ShineId) : IPacketData
{
    public PacketType Type => PacketType.Shine;

    public ShineData(ReadOnlySequence<byte> data)
        : this(data.ReadInt32(0))
    {
    }

    public ReadOnlySequence<byte> AsSequence()
        => new(BitConverter.GetBytes(ShineId));
}
