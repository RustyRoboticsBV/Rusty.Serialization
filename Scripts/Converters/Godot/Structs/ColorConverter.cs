#if GODOT
using Godot;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Gd
{
    /// <summary>
    /// A Godot color converter.
    /// </summary>
    public class ColorConverter : Converter<Color, ColorNode>
    {
        /* Protected method. */
        protected override ColorNode CreateNode(Color obj, CreateNodeContext context)
        {
            return new ColorNode((byte)obj.R8, (byte)obj.G8, (byte)obj.B8, (byte)obj.A8);
        }

        protected override Color CreateObject(ColorNode node, CreateObjectContext context)
        {
            return new Color(node.R / 255f, node.G / 255f, node.B / 255f, node.A / 255f);
        }
    }
}
#endif