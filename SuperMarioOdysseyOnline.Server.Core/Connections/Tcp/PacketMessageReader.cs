using System.Buffers;
using Bedrock.Framework.Protocols;
using SuperMarioOdysseyOnline.Server.Connections.Packets;
using SuperMarioOdysseyOnline.Server.Extensions;

namespace SuperMarioOdysseyOnline.Server.Connections.Tcp;

internal class PacketMessageReader : IMessageReader<IPacket?>
{
    private const byte HeaderSize = 20;

    public bool TryParseMessage(in ReadOnlySequence<byte> input, ref SequencePosition consumed, ref SequencePosition examined, out IPacket? message)
    {
        var reader = new SequenceReader<byte>(input);

        if (!reader.TryReadPacketHeader(out ReadOnlySequence<byte> header, out short dataSize) ||
            !reader.TryReadExact(HeaderSize + dataSize, out ReadOnlySequence<byte> data))
        {
            message = default;

            return false;
        }

        message = CreatePacket(header, data.Slice(HeaderSize));

        consumed = input.GetPosition(reader.Consumed);
        examined = consumed;

        return true;
    }

    private static IPacket CreatePacket(ReadOnlySequence<byte> header, ReadOnlySequence<byte> data)
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
