using System.Buffers;
using SuperMarioOdysseyOnline.Server.Extensions;

namespace SuperMarioOdysseyOnline.Server.Connections.Packets;

public record TagPacket(Guid Id, TagData Data) : IPacket<TagData>, IPacket
{
    public short Type => (short)PacketType.Tag;

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

    public short Size => sizeof(TagFlags) + sizeof(bool) + sizeof(byte) + sizeof(ushort);

    public TimeSpan TimeSpan => TimeSpan.FromMinutes(Minutes).Add(TimeSpan.FromSeconds(Seconds));

    public byte[] ToByteArray()
        => [
            (byte)UpdateType,
            ..BitConverter.GetBytes(IsIt),
            Seconds,
            ..BitConverter.GetBytes(Minutes)
        ];
}

[Flags]
public enum TagFlags : byte
{
    Time = 1,
    State = 2
}
