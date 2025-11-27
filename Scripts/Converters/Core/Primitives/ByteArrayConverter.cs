using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A byte[] converter.
    /// </summary>
    public sealed class ByteArrayConverter : ReferenceConverter<byte[], BinaryNode>
    {
        /* Protected methods. */
        protected override BinaryNode Convert(byte[] obj, Context context) => new(obj);
        protected override byte[] Deconvert(BinaryNode node, Context context) => node.Value;
    }
}