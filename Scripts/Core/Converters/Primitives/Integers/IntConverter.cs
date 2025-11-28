using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An int converter.
    /// </summary>
    public sealed class IntConverter : ValueConverter<int, IntNode>
    {
        /* Protected methods. */
        protected override IntNode ConvertValue(int obj, IConverterScheme scheme) => new(obj);
        protected override int DeconvertValue(IntNode node, IConverterScheme scheme) => (int)node.Value;
    }
}