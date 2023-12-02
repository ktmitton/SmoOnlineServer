using System.Buffers;
using System.Text;
using SuperMarioOdysseyOnline.Server.Extensions;

namespace SuperMarioOdysseyOnline.Server.Packets.Data;

public record ChangeStageData(string Stage, string Id, sbyte Scenario, byte SubScenarioType) : IPacketData
{
    private const int IdSize = 16;

    private const int StageSize = 48;

    public PacketType Type => PacketType.ChangeStage;

    public ChangeStageData(ReadOnlySequence<byte> data)
        : this(
            data.ReadString(0, StageSize),
            data.ReadString(StageSize, IdSize),
            data.ReadSByte(StageSize + IdSize),
            data.ReadByte(StageSize + IdSize + 1)
        )
    {
    }

    public ReadOnlySequence<byte> AsSequence()
        => new([
            ..Encoding.UTF8.GetBytes(Stage.PadRight(StageSize, '\0')),
            ..Encoding.UTF8.GetBytes(Id.PadRight(IdSize, '\0')),
            (byte)Scenario,
            SubScenarioType
        ]);
}
