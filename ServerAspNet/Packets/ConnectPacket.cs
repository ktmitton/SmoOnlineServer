using System.Buffers;
using System.Text;
using SuperMarioOdysseyOnline.Server.Extensions;

namespace SuperMarioOdysseyOnline.Server.Packets;

public record ConnectPacket(Guid Id, ConnectData Data) : IPacket<ConnectData>, IPacket
{
    public PacketType Type => PacketType.Connect;

    public ConnectPacket(Guid id, ReadOnlySequence<byte> data)
        : this(id, new ConnectData(data))
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

    public ReadOnlySequence<byte> AsSequence()
        => new([
            ..BitConverter.GetBytes((int)ConnectionType),
            ..BitConverter.GetBytes(MaxPlayers),
            ..Encoding.UTF8.GetBytes(ClientName)
        ]);
}

public enum ConnectionType {
    FirstConnection,
    Reconnecting
}
