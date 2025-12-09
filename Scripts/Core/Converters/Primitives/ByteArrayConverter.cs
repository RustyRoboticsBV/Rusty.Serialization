using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A byte[] converter.
    /// </summary>
    public sealed class ByteArrayConverter : ReferenceConverter<byte[], BinaryNode>
    {
        /* Protected methods. */
        protected override BinaryNode CreateNode(byte[] obj, IConverterScheme scheme, SymbolTable table) => new(obj);
        protected override byte[] CreateObject(BinaryNode node, IConverterScheme scheme, NodeTree tree) => node.Value;
    }
}