using System.Buffers;
using System.Text;
using SuperMarioOdysseyOnline.Server.Extensions;
using SuperMarioOdysseyOnline.Server.Models;
using SuperMarioOdysseyOnline.Server.Players;

namespace SuperMarioOdysseyOnline.Server.Packets.Data;

public record CostumeData(Costume MariosCostume, Costume CappysCostume) : IPacketData
{
    public PacketType Type => PacketType.Costume;

    private const int NameSize = 32;

    public CostumeData(ReadOnlySequence<byte> data)
        : this(
            new Costume(data.ReadString(0, NameSize)),
            new Costume(data.ReadString(NameSize))
        )
    {
    }

    public CostumeData(IPlayer player)
        : this(player.Mario.Costume, player.Cappy.Costume)
    {
    }

    public ReadOnlySequence<byte> AsSequence()
        => new([
            ..Encoding.UTF8.GetBytes(MariosCostume.Name.PadRight(NameSize, '\0')),
            ..Encoding.UTF8.GetBytes(CappysCostume.Name.PadRight(NameSize, '\0')),
        ]);
}
