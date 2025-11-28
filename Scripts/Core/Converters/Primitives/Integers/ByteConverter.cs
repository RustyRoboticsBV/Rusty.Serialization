using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A byte converter.
    /// </summary>
    public sealed class ByteConverter : ValueConverter<byte, IntNode>
    {
        /* Protected methods. */
        protected override IntNode ConvertValue(byte obj, IConverterScheme scheme) => new(obj);
        protected override byte DeconvertValue(IntNode node, IConverterScheme scheme) => (byte)node.Value;
    }
}