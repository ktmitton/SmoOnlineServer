using System.Buffers;
using SuperMarioOdysseyOnline.Server.Extensions;
using SuperMarioOdysseyOnline.Server.Packets.Data;

namespace SuperMarioOdysseyOnline.Server.Packets;

public record Packet(Guid Id, IPacketData Data)
{
    public Packet(IPacketData data)
        : this(Guid.NewGuid(), data)
    {
    }

    public static Packet Create(ReadOnlySequence<byte> header, ReadOnlySequence<byte> data)
    {
        var id = header.ReadGuid();
        var type = (PacketType)header.ReadInt16(16);

        switch (type)
        {
            case PacketType.CappyRenderData:
                return new Packet(id, new CappyRenderData(data));
            case PacketType.Capture:
                return new Packet(id, new CaptureData(data));
            case PacketType.ChangeStage:
                return new Packet(id, new ChangeStageData(data));
            case PacketType.Connect:
                return new Packet(id, new ConnectData(data));
            case PacketType.Costume:
                return new Packet(id, new CostumeData(data));
            case PacketType.Disconnect:
                return new Packet(id, new DisconnectData(data));
            case PacketType.PlayerStageData:
                return new Packet(id, new PlayerStageData(data));
            case PacketType.Init:
                return new Packet(id, new InitData(data));
            case PacketType.MarioRenderData:
                return new Packet(id, new MarioRenderData(data));
            case PacketType.Shine:
                return new Packet(id, new ShineData(data));
            case PacketType.Tag:
                return new Packet(id, new TagData(data));
            default:
                return new Packet(id, new UnhandledData(data));
        }
    }

    public ReadOnlySpan<byte> AsSpan()
    {
        throw new NotImplementedException("");
    }
}

public interface IPacketData
{
    PacketType Type { get; }

    ReadOnlySequence<byte> AsSequence();
}

public enum PacketType : short
{
    Unknown,
    Init,
    MarioRenderData,
    CappyRenderData,
    PlayerStageData,
    Tag,
    Connect,
    Disconnect,
    Costume,
    Shine,
    Capture,
    ChangeStage,
    Command
}
