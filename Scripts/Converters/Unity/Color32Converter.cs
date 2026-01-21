#if UNITY_5_3_OR_NEWER
using UnityEngine;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity color32 converter.
    /// </summary>
    public class Color32Converter : Converter<Color32, ColorNode>
    {
        /* Protected method. */
        protected override ColorNode CreateNode(Color32 obj, CreateNodeContext context)
        {
            return new ColorNode((obj.r, obj.g, obj.b, obj.a));
        }

        protected override Color32 CreateObject(ColorNode node, CreateObjectContext context)
        {
            return new Color32(node.Value.R, node.Value.G, node.Value.B, node.Value.A);
        }
    }
}
#endif