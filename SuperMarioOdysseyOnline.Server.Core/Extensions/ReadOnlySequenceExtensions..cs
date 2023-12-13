using System.Buffers;
using System.Numerics;
using System.Text;

namespace SuperMarioOdysseyOnline.Server.Core.Extensions;

internal static class ReadOnlySequenceExtensions
{
    public static byte ReadByte(this ReadOnlySequence<byte> sequence, int start = 0)
        => sequence.Slice(start, 1).ToArray()[0];

    public static sbyte ReadSByte(this ReadOnlySequence<byte> sequence, int start = 0)
        => Convert.ToSByte(sequence.ReadByte(start));

    public static bool ReadBoolean(this ReadOnlySequence<byte> sequence, int start = 0)
        => Convert.ToBoolean(sequence.ReadByte(start));

    public static short ReadInt16(this ReadOnlySequence<byte> sequence, int start = 0)
    {
        var slice = sequence.Slice(start, 2);

        var val = BitConverter.ToInt16(slice.ToArray());

        return val;
    }

    public static ushort ReadUInt16(this ReadOnlySequence<byte> sequence, int start = 0)
        => BitConverter.ToUInt16(sequence.Slice(start, 2).ToArray());

    public static int ReadInt32(this ReadOnlySequence<byte> sequence, int start = 0)
        => BitConverter.ToInt32(sequence.Slice(start, 4).ToArray());

    public static float ReadSingle(this ReadOnlySequence<byte> sequence, int start = 0)
        => BitConverter.ToSingle(sequence.Slice(start, 4).ToArray());

    public static float[] ReadSingleArray(this ReadOnlySequence<byte> sequence, int size)
        => sequence.ReadSingleArray(0, size);

    public static float[] ReadSingleArray(this ReadOnlySequence<byte> sequence, int start, int size)
    {
        var result = new float[size];

        for(int i = 0; i < result.Length; ++i)
        {
            result[i] = sequence.ReadSingle(start + (i * 4));
        }

        return result;
    }

    public static Vector3 ReadVector3(this ReadOnlySequence<byte> sequence, int start = 0)
        => new(sequence.ReadSingle(start), sequence.ReadSingle(start + 4), sequence.ReadSingle(start + 8));

    public static Quaternion ReadQuaternion(this ReadOnlySequence<byte> sequence, int start = 0)
        => new(sequence.ReadSingle(start), sequence.ReadSingle(start + 4), sequence.ReadSingle(start + 8), sequence.ReadSingle(start + 12));

    public static Guid ReadGuid(this ReadOnlySequence<byte> sequence, int start = 0)
        => new(sequence.Slice(start, 16).ToArray());

    public static string ReadString(this ReadOnlySequence<byte> sequence)
        => sequence.ReadString(0, sequence.Length);

    public static string ReadString(this ReadOnlySequence<byte> sequence, long size)
        => sequence.ReadString(0, size);

    public static string ReadString(this ReadOnlySequence<byte> sequence, int start)
        => sequence.ReadString(start, sequence.Length - start);

    public static string ReadString(this ReadOnlySequence<byte> sequence, int start, long size)
        => Encoding.UTF8.GetString(sequence.Slice(start, size).ToArray()).TrimEnd('\0');
}
