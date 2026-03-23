using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A byte array converter.
    /// </summary>
    public sealed class ByteArrayConverter : TypedBytesConverter<byte[]>
    {
        /* Protected methods. */
        protected override BytesValue ToBytes(byte[] obj) => obj;
        protected override byte[] FromBytes(BytesValue value) => value;
    }
}