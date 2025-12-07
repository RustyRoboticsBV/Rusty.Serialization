using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A bool converter.
    /// </summary>
    public sealed class BoolConverter : ValueConverter<bool, BoolNode>
    {
        /* Protected methods. */
        protected override BoolNode ConvertValue(bool obj, IConverterScheme scheme, NodeTree tree) => new(obj);
        protected override bool DeconvertValue(BoolNode node, IConverterScheme scheme, NodeTree tree) => node.Value;
    }
}