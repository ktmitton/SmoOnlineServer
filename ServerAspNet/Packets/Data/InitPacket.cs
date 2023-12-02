using System.Buffers;
using SuperMarioOdysseyOnline.Server.Extensions;

namespace SuperMarioOdysseyOnline.Server.Packets.Data;

public record InitData(ushort MaxPlayers) : IPacketData
{
    public PacketType Type => PacketType.Init;

    public InitData(ReadOnlySequence<byte> data)
        : this(data.ReadUInt16(0))
    {
    }

    public ReadOnlySequence<byte> AsSequence()
        => new(BitConverter.GetBytes(MaxPlayers));
}
