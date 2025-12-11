using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A reference tuple type converter.
    /// </summary>
    public sealed class StringConverter : ReferenceConverter<string, StringNode>
    {
        /* Protected methods. */
        protected override string CreateObject(StringNode node, CreateObjectContext context)
        {
            return node.Value;
        }

        protected override StringNode CreateNode(string obj, CreateNodeContext context)
        {
            return new(obj);
        }
    }
}