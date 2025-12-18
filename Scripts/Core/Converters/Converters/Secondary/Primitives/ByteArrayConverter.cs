using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A byte array converter.
    /// </summary>
    public sealed class ByteArrayConverter : Converter<byte[], BinaryNode>
    {
        /* Protected methods. */
        protected override BinaryNode CreateNode(byte[] obj, CreateNodeContext context) => new(obj);
        protected override byte[] CreateObject(BinaryNode node, CreateObjectContext context) => node.Value;
    }
}