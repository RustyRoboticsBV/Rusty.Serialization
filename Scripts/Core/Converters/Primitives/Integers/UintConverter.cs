using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A uint converter.
    /// </summary>
    public sealed class UintConverter : ValueConverter<uint, IntNode>
    {
        /* Protected methods. */
        protected override IntNode ConvertValue(uint obj, IConverterScheme scheme) => new(obj);
        protected override uint DeconvertValue(IntNode node, IConverterScheme scheme) => (uint)node.Value;
    }
}