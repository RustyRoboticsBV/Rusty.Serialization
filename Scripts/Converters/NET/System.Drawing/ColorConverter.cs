#if NETCOREAPP2_0_OR_GREATER
using System.Drawing;
using Rusty.Serialization.Core.Conversion;
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
            return new ColorNode(obj.R, obj.G, obj.B, obj.A);
        }

        protected override Color CreateObject(ColorNode node, CreateObjectContext context)
        {
            return Color.FromArgb(node.Value.a, node.Value.r, node.Value.g, node.Value.b);
        }
    }
}
#endif