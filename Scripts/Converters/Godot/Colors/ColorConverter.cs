#if GODOT
using Godot;
using Rusty.Serialization.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Gd
{
    /// <summary>
    /// A Godot.Color converter.
    /// </summary>
    public sealed class ColorConverter : ValueConverter<Color, ColorNode>
    {
        /* Protected methods. */
        protected override ColorNode Convert(Color obj, Context context)
            => new((byte)obj.R8, (byte)obj.G8, (byte)obj.B8, (byte)obj.A8);
        protected override Color Deconvert(ColorNode node, Context context)
            => new Color(node.R / 255f, node.G / 255f, node.B / 255f, node.A / 255f);
    }
}
#endif