using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A ushort converter.
    /// </summary>
    public sealed class UshortConverter : ValueConverter<ushort, IntNode>
    {
        /* Protected methods. */
        protected override IntNode ConvertValue(ushort obj, IConverterScheme scheme, SymbolTable table) => new(obj);
        protected override ushort DeconvertValue(IntNode node, IConverterScheme scheme, NodeTree tree) => (ushort)node.Value;
    }
}