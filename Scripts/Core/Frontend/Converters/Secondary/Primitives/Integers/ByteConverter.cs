using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A byte converter.
    /// </summary>
    public sealed class ByteConverter : TypedIntConverter<byte>
    {
        /* Protected methods. */
        protected override IntValue ToInt(byte obj) => (IntValue)obj;
        protected override byte FromInt(IntValue value) => (byte)value;
    }
}