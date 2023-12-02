using System.Buffers;
using SuperMarioOdysseyOnline.Server.Extensions;

namespace SuperMarioOdysseyOnline.Server.Packets.Data;

public record TagData(TagFlags UpdateType, bool IsIt, byte Seconds, ushort Minutes) : IPacketData
{
    public PacketType Type => PacketType.Tag;

    public TagData(ReadOnlySequence<byte> data)
        : this(
            (TagFlags)data.ReadByte(),
            data.ReadBoolean(1),
            data.ReadByte(2),
            data.ReadUInt16(3)
        )
    {
    }

    public ReadOnlySequence<byte> AsSequence()
        => new([
            (byte)UpdateType,
            ..BitConverter.GetBytes(IsIt),
            Seconds,
            ..BitConverter.GetBytes(Minutes)
        ]);
}

[Flags]
public enum TagFlags : byte
{
    Time = 1,
    State = 2
}
