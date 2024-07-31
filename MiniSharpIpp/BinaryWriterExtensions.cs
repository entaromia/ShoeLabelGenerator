using System.Buffers.Binary;

namespace MiniSharpIpp
{
    public static class BinaryWriterExtensions
    {
        public static void WriteBigEndian(this BinaryWriter writer, short value)
        {
            if (BitConverter.IsLittleEndian)
            {
                writer.Write(BinaryPrimitives.ReverseEndianness(value));
            }
            else
            {
                writer.Write(value);
            }
        }

        public static void WriteBigEndian(this BinaryWriter writer, int value)
        {
            if (BitConverter.IsLittleEndian)
            {
                writer.Write(BinaryPrimitives.ReverseEndianness(value));
            }
            else
            {
                writer.Write(value);
            }
        }
    }
}
