using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A character converter.
    /// </summary>
    public sealed class CharConverter : Converter<char, CharNode>
    {
        /* Protected methods. */
        protected override CharNode CreateNode(char obj, CreateNodeContext context) => new(obj.ToString());
        protected override char CreateObject(CharNode node, CreateObjectContext context) => (char)node.Value;
    }
}