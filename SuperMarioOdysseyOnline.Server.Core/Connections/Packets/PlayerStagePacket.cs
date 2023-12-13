using System.Buffers;
using System.Text;
using SuperMarioOdysseyOnline.Server.Core.Extensions;
using SuperMarioOdysseyOnline.Server.Core.Lobby;
using SuperMarioOdysseyOnline.Server.Core.Models;

namespace SuperMarioOdysseyOnline.Server.Core.Connections.Packets;

public record PlayerStagePacket(Guid Id, PlayerStageData Data) : IPacket<PlayerStageData>, IPacket
{
    public short Type => (short)PacketType.PlayerStageData;

    public PlayerStagePacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new PlayerStageData(data))
    {
    }

    public PlayerStagePacket(IPlayer player)
        : this(Guid.NewGuid(), new PlayerStageData(player))
    {
    }

    IPacketData IPacket<IPacketData>.Data => Data;
}

public record PlayerStageData(Stage Stage) : IPacketData
{
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

    public short Size => (short)(2 + Stage.Name.Length);

    public byte[] ToByteArray()
        => [
            ..BitConverter.GetBytes(Stage.Is2d),
            Stage.Scenario,
            ..Encoding.UTF8.GetBytes(Stage.Name)
        ];
}
