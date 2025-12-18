using System.Drawing;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Dotnet
{
    /// <summary>
    /// A color converter.
    /// </summary>
    public sealed class ColorConverter : Converter<Color, ColorNode>
    {
        /* Protected methods. */
        protected override ColorNode CreateNode(Color obj, CreateNodeContext context) => new(obj.R, obj.G, obj.B, obj.A);
        protected override Color CreateObject(ColorNode node, CreateObjectContext context) => Color.FromArgb(node.A, node.R, node.G, node.B);
    }
}