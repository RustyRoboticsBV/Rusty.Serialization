#if NET5_0_OR_GREATER
using System.Text;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Text.Rune converter.
    /// </summary>
    public sealed class RuneConverter : ValueConverter<Rune, CharNode>
    {
        /* Protected methods. */
        protected override CharNode ConvertValue(Rune obj, IConverterScheme scheme, SymbolTable table) => new(obj.Value);
        protected override Rune DeconvertValue(CharNode node, IConverterScheme scheme, NodeTree tree) => new(node.Value);
    }
}
#endif