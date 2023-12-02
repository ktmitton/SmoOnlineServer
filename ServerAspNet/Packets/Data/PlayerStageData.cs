using System.Buffers;
using System.Text;
using SuperMarioOdysseyOnline.Server.Extensions;
using SuperMarioOdysseyOnline.Server.Models;
using SuperMarioOdysseyOnline.Server.Players;

namespace SuperMarioOdysseyOnline.Server.Packets.Data;

public record PlayerStageData(Stage Stage) : IPacketData
{
    public PacketType Type => PacketType.PlayerStageData;

    public PlayerStageData(ReadOnlySequence<byte> data)
        : this(
            new Stage(
                data.ReadByte(1),
                data.ReadString(2),
                data.ReadBoolean(0)
            )
        )
    {
    }

    public PlayerStageData(IPlayer player)
        : this(player.Stage)
    {
    }

    public ReadOnlySequence<byte> AsSequence()
        => new([
            ..BitConverter.GetBytes(Stage.Is2d),
            Stage.Id,
            ..Encoding.UTF8.GetBytes(Stage.Name)
        ]);
}
