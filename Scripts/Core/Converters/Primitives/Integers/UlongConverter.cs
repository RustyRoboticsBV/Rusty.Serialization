using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A ulong converter.
    /// </summary>
    public sealed class UlongConverter : ValueConverter<ulong, IntNode>
    {
        /* Protected methods. */
        protected override IntNode ConvertValue(ulong obj, IConverterScheme scheme) => new(obj);
        protected override ulong DeconvertValue(IntNode node, IConverterScheme scheme) => (ulong)node.Value;
    }
}