using System.Buffers;
using Bedrock.Framework.Protocols;
using Microsoft.AspNetCore.Connections;
using SuperMarioOdysseyOnline.Server.Extensions;
using SuperMarioOdysseyOnline.Server.Packets;

namespace SuperMarioOdysseyOnline.Server.Connections;

internal class TcpPacketConnection(ConnectionContext connectionContext) : IPacketConnection, IMessageReader<IPacket?>, IMessageWriter<IPacket>
{
    private const byte HeaderSize = 20;

    private readonly CancellationToken _connectionCancellationToken = connectionContext.ConnectionClosed;

    private readonly ProtocolReader _reader = connectionContext.CreateReader();

    private readonly ProtocolWriter _writer = connectionContext.CreateWriter();

    public ConnectionContext ConnectionContext => connectionContext;

    public async Task<T> ReceiveNextPacketAsync<T>(CancellationToken cancellationToken) where T : IPacket
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var packet = await ReceiveNextPacketAsync(cancellationToken);
            if (packet is T converted)
            {
                return converted;
            }
        }
    }

    public async Task<IPacket> ReceiveNextPacketAsync(CancellationToken cancellationToken)
    {
        var packet = (await _reader.ReadAsync(this, CreateLinkedToken(cancellationToken))).Message;

        _reader.Advance();

        return packet;
    }

    public Task SendPacketAsync(IPacket packet, CancellationToken cancellationToken)
        => _writer.WriteAsync(this, packet, CreateLinkedToken(cancellationToken)).AsTask();

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

    public void WriteMessage(IPacket message, IBufferWriter<byte> output)
        => output.Write(message.AsSpan());

    private CancellationToken CreateLinkedToken(CancellationToken cancellationToken)
        => CancellationTokenSource.CreateLinkedTokenSource(_connectionCancellationToken, cancellationToken).Token;

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
