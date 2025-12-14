using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A string converter.
    /// </summary>
    public sealed class StringConverter : ReferenceConverter<string, StringNode>
    {
        /* Protected methods. */
        protected override StringNode CreateNode(string obj, CreateNodeContext context) => new(obj);
        protected override string CreateObject(StringNode node, CreateObjectContext context) => node.Value;
    }
}