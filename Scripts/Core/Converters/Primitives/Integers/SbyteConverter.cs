using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An sbyte converter.
    /// </summary>
    public sealed class SbyteConverter : ValueConverter<sbyte, IntNode>
    {
        /* Protected methods. */
        protected override IntNode ConvertValue(sbyte obj, IConverterScheme scheme) => new(obj);
        protected override sbyte DeconvertValue(IntNode node, IConverterScheme scheme) => (sbyte)node.Value;
    }
}