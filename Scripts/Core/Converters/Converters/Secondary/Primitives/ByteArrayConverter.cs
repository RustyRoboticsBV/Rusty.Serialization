using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A byte array converter.
    /// </summary>
    public sealed class ByteArrayConverter : Converter<byte[], BytesNode>
    {
        /* Protected methods. */
        protected override BytesNode CreateNode(byte[] obj, CreateNodeContext context) => new(obj);
        protected override byte[] CreateObject(BytesNode node, CreateObjectContext context) => node.Name;
    }
}