using System.Buffers;
using Bedrock.Framework.Protocols;
using SuperMarioOdysseyOnline.Server.Connections;
using SuperMarioOdysseyOnline.Server.Extensions;
using SuperMarioOdysseyOnline.Server.Packets;
using SuperMarioOdysseyOnline.Server.Packets.Data;
using SuperMarioOdysseyOnline.Server.UpdateStrategies;

namespace SuperMarioOdysseyOnline.Server.Players;

public static class TcpPlayerConnectionServiceExtensions
{
    public static IServiceCollection AddTcpPlayerConnection(this IServiceCollection services)
        => services.AddScoped<TcpPlayerConnection>();
}

public class TcpPlayerConnection
    : IPlayerConnection, IMessageReader<Packet?>, IMessageWriter<Packet>
{
    private readonly CancellationToken _connectionCancellationToken;

    private readonly ProtocolReader _reader;

    private readonly ProtocolWriter _writer;

    private readonly IPlayerManager _playerManager;

    public IPlayer? Player { get; private set; }

    public IUpdateStrategy? PreferredUpdateStrategy => AvailableUpdateStrategies.OrderBy(x => x.ConnectionRating).FirstOrDefault();

    public IEnumerable<IUpdateStrategy> AvailableUpdateStrategies { get; }

    public TcpPlayerConnection(IConnectionContextAccessor connectionContextAccessor, IPlayerManager playerManager, IEnumerable<IUpdateStrategy> updateStrategies)
    {
        ArgumentNullException.ThrowIfNull(connectionContextAccessor.ConnectionContext);

        _connectionCancellationToken = connectionContextAccessor.ConnectionContext.ConnectionClosed;
        _reader = connectionContextAccessor.ConnectionContext.CreateReader();
        _writer = connectionContextAccessor.ConnectionContext.CreateWriter();

        _playerManager = playerManager;

        AvailableUpdateStrategies = updateStrategies;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var packet = await ReceiveNextPacketAsync(cancellationToken);

        if (packet.Data is ConnectData connectData)
        {
            Player = _playerManager.GetPlayer(packet.Id, connectData.ClientName);
        }

        foreach(var strategy in AvailableUpdateStrategies)
        {
            await strategy.RefreshConnectionRatingAsync(cancellationToken);
        }
    }

    public async Task<Packet> ReceiveNextPacketAsync(CancellationToken cancellationToken)
        => (await _reader.ReadAsync(this, CreateLinkedToken(cancellationToken))).Message;

    public Task SendPacketAsync(Packet packet, CancellationToken cancellationToken)
        => _writer.WriteAsync(this, packet, CreateLinkedToken(cancellationToken)).AsTask();

    public bool TryParseMessage(in ReadOnlySequence<byte> input, ref SequencePosition consumed, ref SequencePosition examined, out Packet? message)
    {
        var reader = new SequenceReader<byte>(input);

        if (!reader.TryReadPacket(out message))
        {
            return false;
        }

        consumed = input.GetPosition(reader.Consumed);
        examined = consumed;

        return true;
    }

    public void WriteMessage(Packet message, IBufferWriter<byte> output)
        => output.Write(message.AsSpan());

    private CancellationToken CreateLinkedToken(CancellationToken cancellationToken)
        => CancellationTokenSource.CreateLinkedTokenSource(_connectionCancellationToken, cancellationToken).Token;
}
