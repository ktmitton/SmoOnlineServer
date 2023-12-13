using Bedrock.Framework.Protocols;
using Microsoft.AspNetCore.Connections;
using SuperMarioOdysseyOnline.Server.Core.Connections.Packets;

namespace SuperMarioOdysseyOnline.Server.Core.Connections.Tcp;

internal class TcpPacketConnection(
    ConnectionContext connectionContext,
    IMessageReader<IPacket?> messageReader,
    IMessageWriter<IPacket> messageWriter
) : IPacketConnection
{
    private readonly CancellationToken _connectionCancellationToken = connectionContext.ConnectionClosed;

    private readonly ProtocolReader _reader = connectionContext.CreateReader();

    private readonly ProtocolWriter _writer = connectionContext.CreateWriter();

    private readonly IMessageReader<IPacket?> _messageReader = messageReader;

    private readonly IMessageWriter<IPacket> _messageWriter = messageWriter;

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
        var packet = (await _reader.ReadAsync(_messageReader, CreateLinkedToken(cancellationToken))).Message;

        _reader.Advance();

        return packet;
    }

    public Task SendPacketAsync(IPacket packet, CancellationToken cancellationToken)
        => _writer.WriteAsync(_messageWriter, packet, CreateLinkedToken(cancellationToken)).AsTask();

    private CancellationToken CreateLinkedToken(CancellationToken cancellationToken)
        => CancellationTokenSource.CreateLinkedTokenSource(_connectionCancellationToken, cancellationToken).Token;
}
