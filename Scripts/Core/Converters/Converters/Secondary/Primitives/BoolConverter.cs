using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A boolean converter.
    /// </summary>
    public sealed class BoolConverter : Converter<bool, BoolNode>
    {
        /* Protected methods. */
        protected override BoolNode CreateNode(bool obj, CreateNodeContext context) => new(obj);
        protected override bool CreateObject(BoolNode node, CreateObjectContext context) => (bool)node.Value;
    }
}