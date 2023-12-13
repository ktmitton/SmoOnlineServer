using System.Buffers;

namespace SuperMarioOdysseyOnline.Server.Core.Extensions;

internal static class SequenceReaderExtensions
{
    private const byte HeaderSize = 20;

    public static bool TryReadPacketHeader(this SequenceReader<byte> reader, out ReadOnlySequence<byte> sequence, out short dataSize)
    {
        if (!reader.TryReadExact(HeaderSize, out sequence))
        {
            dataSize = default;

            return false;
        }

        var a = sequence.Slice(16).ReadInt16();
        dataSize = sequence.Slice(18).ReadInt16();

        return true;
    }
}
