using System.Drawing;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A System.Drawing.Color converter.
    /// </summary>
    public sealed class ColorConverter : ValueConverter<Color, ColorNode>
    {
        /* Protected methods. */
        protected override ColorNode Convert(Color obj, Context context) => new(obj.R, obj.G, obj.B, obj.A);
        protected override Color Deconvert(ColorNode node, Context context) => Color.FromArgb(node.A, node.R, node.G, node.B);
    }
}