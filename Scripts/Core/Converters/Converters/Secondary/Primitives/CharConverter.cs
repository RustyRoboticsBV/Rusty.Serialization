using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A character converter.
    /// </summary>
    public sealed class CharConverter : Converter<char, CharNode>
    {
        /* Protected methods. */
        protected override CharNode CreateNode(char obj, CreateNodeContext context) => new(obj.ToString());
        protected override char CreateObject(CharNode node, CreateObjectContext context) => node.Value[0];
    }
}