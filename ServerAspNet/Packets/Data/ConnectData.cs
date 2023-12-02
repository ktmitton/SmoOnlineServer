using System.Buffers;
using System.Text;
using SuperMarioOdysseyOnline.Server.Extensions;

namespace SuperMarioOdysseyOnline.Server.Packets.Data;

public record ConnectData(ConnectionType ConnectionType, ushort MaxPlayers, string ClientName) : IPacketData
{
    public PacketType Type => PacketType.Connect;

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
