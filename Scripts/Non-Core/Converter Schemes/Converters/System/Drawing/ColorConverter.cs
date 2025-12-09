using System.Drawing;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Drawing.Color converter.
    /// </summary>
    public sealed class ColorConverter : ValueConverter<Color, ColorNode>
    {
        /* Protected methods. */
        protected override ColorNode ConvertValue(Color obj, IConverterScheme scheme, SymbolTable table) => new(obj.R, obj.G, obj.B, obj.A);
        protected override Color DeconvertValue(ColorNode node, IConverterScheme scheme, ParsingTable table) => Color.FromArgb(node.A, node.R, node.G, node.B);
    }
}