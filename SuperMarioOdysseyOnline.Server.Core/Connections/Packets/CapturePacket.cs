using System.Buffers;
using System.Text;
using SuperMarioOdysseyOnline.Server.Extensions;
using SuperMarioOdysseyOnline.Server.Lobbies;
using SuperMarioOdysseyOnline.Server.Models;

namespace SuperMarioOdysseyOnline.Server.Connections.Packets;

public record CapturePacket(Guid Id, CaptureData Data) : IPacket<CaptureData>, IPacket
{
    public short Type => (short)PacketType.Capture;

    IPacketData IPacket<IPacketData>.Data => Data;

    public CapturePacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new CaptureData(data))
    {
    }

    public CapturePacket(IPlayer player)
        : this(Guid.NewGuid(), new CaptureData(player))
    {
    }
}

public record CaptureData(CapturedEntity CapturedEntity) : IPacketData
{
    public CaptureData(ReadOnlySequence<byte> data)
        : this(new CapturedEntity(data.ReadString()))
    {
    }

    public CaptureData(IPlayer player)
        : this(player.Mario.CapturedEntity ?? new())
    {
    }

    public short Size => (short)CapturedEntity.Name.Length;

    public byte[] ToByteArray()
        => Encoding.UTF8.GetBytes(CapturedEntity.Name);
}
