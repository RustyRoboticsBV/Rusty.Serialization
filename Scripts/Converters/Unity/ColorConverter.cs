#if UNITY_5_3_OR_NEWER
using UnityEngine;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity color converter.
    /// </summary>
    public class ColorConverter : Converter<Color, ColorNode>
    {
        /* Protected method. */
        protected override ColorNode CreateNode(Color obj, CreateNodeContext context)
        {
            return new ColorNode(
                ((byte)(obj.r * 255),
                (byte)(obj.g * 255),
                (byte)(obj.b * 255),
                (byte)(obj.a * 255))
            );
        }

        protected override Color CreateObject(ColorNode node, CreateObjectContext context)
        {
            return new Color(node.Value.R / 255f, node.Value.G / 255f, node.Value.B / 255f, node.Value.A / 255f);
        }
    }
}
#endif