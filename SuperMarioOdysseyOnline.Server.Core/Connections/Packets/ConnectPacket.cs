using System.Buffers;
using System.Text;
using SuperMarioOdysseyOnline.Server.Core.Extensions;
using SuperMarioOdysseyOnline.Server.Lobbies;

namespace SuperMarioOdysseyOnline.Server.Core.Connections.Packets;

public record ConnectPacket(Guid Id, ConnectData Data) : IPacket<ConnectData>, IPacket
{
    public short Type => (short)PacketType.Connect;

    public ConnectPacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new ConnectData(data))
    {
    }

    public ConnectPacket(IPlayer player)
        : this(player.Id, new ConnectData(ConnectionType.FirstConnection, default, player.Name))
    {
    }

    IPacketData IPacket<IPacketData>.Data => Data;
}

public record ConnectData(ConnectionType ConnectionType, ushort MaxPlayers, string ClientName) : IPacketData
{
    public ConnectData(ReadOnlySequence<byte> data)
        : this((ConnectionType)data.ReadInt32(0), data.ReadUInt16(4), data.ReadString(6))
    {
    }

    public short Size => (short)(sizeof(int) + sizeof(ushort) + ClientName.Length);

    public byte[] ToByteArray()
        => [
            ..BitConverter.GetBytes((int)ConnectionType),
            ..BitConverter.GetBytes(MaxPlayers),
            ..Encoding.UTF8.GetBytes(ClientName)
        ];
}

public enum ConnectionType {
    FirstConnection,
    Reconnecting
}
