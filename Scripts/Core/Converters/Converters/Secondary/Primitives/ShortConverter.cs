using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A short converter.
    /// </summary>
    public sealed class ShortConverter : Converter<short, IntNode>
    {
        /* Protected methods. */
        protected override IntNode CreateNode(short obj, CreateNodeContext context) => new IntNode(obj);
        protected override short CreateObject(IntNode node, CreateObjectContext context) => (short)node.Value;
    }
}