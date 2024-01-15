using System.Buffers;
using System.Text;
using SuperMarioOdysseyOnline.Server.Extensions;
using SuperMarioOdysseyOnline.Server.Lobbies;
using SuperMarioOdysseyOnline.Server.World;

namespace SuperMarioOdysseyOnline.Server.Connections.Packets;

public record ChangeStagePacket(Guid Id, ChangeStageData Data) : IPacket<ChangeStageData>, IPacket
{
    public short Type => (short)PacketType.ChangeStage;

    public ChangeStagePacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new ChangeStageData(data))
    {
    }

    public ChangeStagePacket(IPlayer player, string stageName)
        : this(player.Id, new ChangeStageData(player.Id, stageName, -1, 0))
    {
    }

    public ChangeStagePacket(IPlayer player, Stage stage)
        : this(player, stage.ToString())
    {
    }


    IPacketData IPacket<IPacketData>.Data => Data;
}

public record ChangeStageData(Guid PlayerId, string StageName, sbyte Scenario, byte SubScenarioType) : IPacketData
{
    private const byte IdSize = 16;

    private const byte StageSize = 48;

    private const byte Padding = 0;

    public ChangeStageData(ReadOnlySequence<byte> data)
        : this(
            data.ReadGuid(StageSize),
            data.ReadString(0, StageSize),
            data.ReadSByte(StageSize + IdSize),
            data.ReadByte(StageSize + IdSize + 1)
        )
    {
    }

    public short Size => IdSize + StageSize + 4;

    public byte[] ToByteArray()
        => [
            ..Encoding.UTF8.GetBytes(StageName.PadRight(StageSize, '\0')),
            ..PlayerId.ToByteArray(),
            (byte)Scenario,
            SubScenarioType,
            Padding,
            Padding
        ];
}
