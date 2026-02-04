using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A sbyte converter.
    /// </summary>
    public sealed class SbyteConverter : Converter<sbyte, IntNode>
    {
        /* Protected methods. */
        protected override IntNode CreateNode(sbyte obj, CreateNodeContext context) => new IntNode(obj);
        protected override sbyte CreateObject(IntNode node, CreateObjectContext context) => (sbyte)node.Value;
    }
}