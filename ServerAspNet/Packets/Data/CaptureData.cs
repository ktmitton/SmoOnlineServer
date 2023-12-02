using System.Buffers;
using System.Text;
using SuperMarioOdysseyOnline.Server.Extensions;
using SuperMarioOdysseyOnline.Server.Models;
using SuperMarioOdysseyOnline.Server.Players;

namespace SuperMarioOdysseyOnline.Server.Packets.Data;

public record CaptureData(CapturedEntity CapturedEntity) : IPacketData
{
    public PacketType Type => PacketType.Capture;

    public CaptureData(ReadOnlySequence<byte> data)
        : this(new CapturedEntity(data.ReadString()))
    {
    }

    public CaptureData(IPlayer player)
        : this(player.Mario.CapturedEntity ?? new())
    {
    }

    public ReadOnlySequence<byte> AsSequence()
        => new([
            ..Encoding.UTF8.GetBytes(CapturedEntity.Name)
        ]);
}
