using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A byte[] converter.
    /// </summary>
    public sealed class ByteArrayConverter : ReferenceConverter<byte[], BinaryNode>
    {
        /* Protected methods. */
        protected override BinaryNode ConvertRef(byte[] obj, IConverterScheme scheme, NodeTree tree) => new(obj);
        protected override byte[] DeconvertRef(BinaryNode node, IConverterScheme scheme, NodeTree tree) => node.Value;
    }
}