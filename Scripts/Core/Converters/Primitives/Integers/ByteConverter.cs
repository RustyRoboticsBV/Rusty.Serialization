using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A byte converter.
    /// </summary>
    public sealed class ByteConverter : ValueConverter<byte, IntNode>
    {
        /* Protected methods. */
        protected override IntNode Convert(byte obj, Context context) => new(obj);
        protected override byte Deconvert(IntNode node, Context context) => (byte)node.Value;
    }
}