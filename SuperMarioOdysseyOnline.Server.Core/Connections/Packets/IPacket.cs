using System.Buffers;
using SuperMarioOdysseyOnline.Server.Extensions;

namespace SuperMarioOdysseyOnline.Server.Connections.Packets;

public interface IPacket : IPacket<IPacketData>
{
    virtual byte[] ToByteArray()
    {
        var id = Id.ToByteArray();
        var type = BitConverter.GetBytes((short)Type);
        var size = BitConverter.GetBytes(Data.Size);
        var data = Data.ToByteArray();

        return [
            ..id,
            ..type,
            ..size,
            ..data
        ];
    }

    static virtual IPacket Create(ReadOnlySequence<byte> header, ReadOnlySequence<byte> data)
    {
        var id = header.ReadGuid();
        var type = (PacketType)header.ReadInt16(16);

        return type switch
        {
            PacketType.CappyRenderData => new CappyRenderPacket(id, data),
            PacketType.Capture => new CapturePacket(id, data),
            PacketType.ChangeStage => new ChangeStagePacket(id, data),
            PacketType.Connect => new ConnectPacket(id, data),
            PacketType.Costume => new CostumePacket(id, data),
            PacketType.Disconnect => new DisconnectPacket(id, data),
            PacketType.PlayerStageData => new PlayerStagePacket(id, data),
            PacketType.Init => new InitPacket(id, data),
            PacketType.MarioRenderData => new MarioRenderPacket(id, data),
            PacketType.Shine => new ShinePacket(id, data),
            PacketType.Tag => new TagPacket(id, data),
            _ => new UnhandledPacket(id, data),
        };
    }
}

public interface IPacket<T> where T : IPacketData
{
    Guid Id { get; }

    short Type { get; }

    T Data { get; }
}

public interface IPacketData
{
    byte[] ToByteArray();

    short Size { get; }
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
