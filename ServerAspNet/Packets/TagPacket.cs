using System.Buffers;
using SuperMarioOdysseyOnline.Server.Extensions;

namespace SuperMarioOdysseyOnline.Server.Packets;

public record TagPacket(Guid Id, TagData Data) : IPacket<TagData>, IPacket
{
    public PacketType Type => PacketType.Tag;

    public TagPacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new TagData(data))
    {
    }

    IPacketData IPacket<IPacketData>.Data => Data;
}

public record TagData(TagFlags UpdateType, bool IsIt, byte Seconds, ushort Minutes) : IPacketData
{
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
