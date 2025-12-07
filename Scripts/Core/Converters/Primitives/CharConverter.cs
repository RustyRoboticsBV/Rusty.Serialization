using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A char converter.
    /// </summary>
    public sealed class CharConverter : ValueConverter<char, CharNode>
    {
        /* Protected methods. */
        protected override CharNode ConvertValue(char obj, IConverterScheme scheme, NodeTree tree) => new(obj);
        protected override char DeconvertValue(CharNode node, IConverterScheme scheme, NodeTree tree) => (char)node.Value;
    }
}