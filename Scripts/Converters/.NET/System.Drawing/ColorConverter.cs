using System.Drawing;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET color converter.
    /// </summary>
    public class ColorConverter : Converter<Color, ColorNode>
    {
        /* Protected method. */
        protected override ColorNode CreateNode(Color obj, CreateNodeContext context)
        {
            return new(obj.R, obj.G, obj.B, obj.A);
        }

        protected override Color CreateObject(ColorNode node, CreateObjectContext context)
        {
            return Color.FromArgb(node.A, node.R, node.G, node.B);
        }
    }
}