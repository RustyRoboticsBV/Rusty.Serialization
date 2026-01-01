using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A short converter.
    /// </summary>
    public sealed class ShortConverter : Converter<short, IntNode>
    {
        /* Protected methods. */
        protected override IntNode CreateNode(short obj, CreateNodeContext context) => new(obj);
        protected override short CreateObject(IntNode node, CreateObjectContext context) => short.Parse(node.Value);
    }
}