using System.Buffers;
using Bedrock.Framework.Protocols;
using SuperMarioOdysseyOnline.Server.Core.Connections.Packets;

namespace SuperMarioOdysseyOnline.Server.Core.Connections.Tcp;

internal class PacketMessageWriter : IMessageWriter<IPacket>
{
    public void WriteMessage(IPacket message, IBufferWriter<byte> output)
        => output.Write(message.ToByteArray());
}
