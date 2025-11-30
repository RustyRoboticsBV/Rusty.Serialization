#if UNITY_5_3_OR_NEWER
using UnityEngine;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.Color converter.
    /// </summary>
    public sealed class ColorConverter : ValueConverter<Color, ColorNode>
    {
        /* Protected methods. */
        protected override ColorNode ConvertValue(Color obj, IConverterScheme scheme)
        {
            Color32 col32 = obj;
            return new(col32.r, col32.g, col32.b, col32.a);
        }

        protected override Color DeconvertValue(ColorNode node, IConverterScheme scheme)
            => new Color32(node.R, node.G, node.B, node.A);
    }
}
#endif