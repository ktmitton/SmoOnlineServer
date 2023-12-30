using System.Buffers;
using Bedrock.Framework.Protocols;
using SuperMarioOdysseyOnline.Server.Connections.Packets;

namespace SuperMarioOdysseyOnline.Server.Connections.Tcp;

internal class PacketMessageWriter : IMessageWriter<IPacket>
{
    public void WriteMessage(IPacket message, IBufferWriter<byte> output)
        => output.Write(message.ToByteArray());
}
