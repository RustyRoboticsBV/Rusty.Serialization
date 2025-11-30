#if UNITY_5_3_OR_NEWER
using UnityEngine;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.Color32 converter.
    /// </summary>
    public sealed class Color32Converter : ValueConverter<Color32, ColorNode>
    {
        /* Protected methods. */
        protected override ColorNode ConvertValue(Color32 obj, IConverterScheme scheme)
            => new(obj.r, obj.g, obj.b, obj.a);

        protected override Color32 DeconvertValue(ColorNode node, IConverterScheme scheme)
            => new(node.R, node.G, node.B, node.A);
    }
}
#endif