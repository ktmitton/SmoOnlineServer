using System.Buffers;
using System.Text;
using SuperMarioOdysseyOnline.Server.Core.Extensions;

namespace SuperMarioOdysseyOnline.Server.Core.Connections.Packets;

public record ChangeStagePacket(Guid Id, ChangeStageData Data) : IPacket<ChangeStageData>, IPacket
{
    public short Type => (short)PacketType.ChangeStage;

    public ChangeStagePacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new ChangeStageData(data))
    {
    }

    IPacketData IPacket<IPacketData>.Data => Data;
}

public record ChangeStageData(string Stage, string Id, sbyte Scenario, byte SubScenarioType) : IPacketData
{
    private const byte IdSize = 16;

    private const byte StageSize = 48;

    public ChangeStageData(ReadOnlySequence<byte> data)
        : this(
            data.ReadString(0, StageSize),
            data.ReadString(StageSize, IdSize),
            data.ReadSByte(StageSize + IdSize),
            data.ReadByte(StageSize + IdSize + 1)
        )
    {
    }

    public short Size => StageSize * 2 + sizeof(sbyte) + sizeof(byte);

    public byte[] ToByteArray()
        => [
            ..Encoding.UTF8.GetBytes(Stage.PadRight(StageSize, '\0')),
            ..Encoding.UTF8.GetBytes(Id.PadRight(IdSize, '\0')),
            (byte)Scenario,
            SubScenarioType
        ];
}
