using System.Buffers;
using SuperMarioOdysseyOnline.Server.Extensions;
using SuperMarioOdysseyOnline.Server.Lobbies;

namespace SuperMarioOdysseyOnline.Server.Connections.Packets;

public record TagPacket(Guid Id, TagData Data) : IPacket<TagData>, IPacket
{
    public short Type => (short)PacketType.Tag;

    public TagPacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new TagData(data))
    {
    }

    public TagPacket(IPlayer player, bool isSeeking)
        : this(player.Id, new TagData(TagFlags.State, isSeeking, default, default))
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

    public short Size => sizeof(TagFlags) + sizeof(bool) + sizeof(byte) + 1 + sizeof(ushort);

    public TimeSpan TimeSpan => TimeSpan.FromMinutes(Minutes).Add(TimeSpan.FromSeconds(Seconds));

    private const byte Padding = 0;

    public byte[] ToByteArray()
        => [
            (byte)UpdateType,
            ..BitConverter.GetBytes(IsIt),
            Seconds,
            Padding,
            ..BitConverter.GetBytes(Minutes)
        ];
}

[Flags]
public enum TagFlags : byte
{
    Time = 1,
    State = 2
}
