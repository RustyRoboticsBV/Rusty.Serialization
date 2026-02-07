using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A byte converter.
    /// </summary>
    public sealed class ByteConverter : Converter<byte, IntNode>
    {
        /* Protected methods. */
        protected override IntNode CreateNode(byte obj, CreateNodeContext context) => new IntNode(obj);
        protected override byte CreateObject(IntNode node, CreateObjectContext context) => (byte)node.Name;
    }
}