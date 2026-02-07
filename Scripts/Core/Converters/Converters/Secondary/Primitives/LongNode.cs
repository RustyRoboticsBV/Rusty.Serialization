using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A long converter.
    /// </summary>
    public sealed class LongConverter : Converter<long, IntNode>
    {
        /* Protected methods. */
        protected override IntNode CreateNode(long obj, CreateNodeContext context) => new IntNode(obj);
        protected override long CreateObject(IntNode node, CreateObjectContext context) => (long)node.Name;
    }
}