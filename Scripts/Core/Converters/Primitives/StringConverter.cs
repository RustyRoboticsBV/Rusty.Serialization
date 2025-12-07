using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A string converter.
    /// </summary>
    public sealed class StringConverter : ReferenceConverter<string, StringNode>
    {
        /* Protected methods. */
        protected override StringNode ConvertRef(string obj, IConverterScheme scheme, NodeTree tree) => new(obj);
        protected override string DeconvertRef(StringNode node, IConverterScheme scheme, NodeTree tree) => node.Value;
    }
}