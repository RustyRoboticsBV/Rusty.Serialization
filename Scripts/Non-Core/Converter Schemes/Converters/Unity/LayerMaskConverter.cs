#if UNITY_5_3_OR_NEWER
using UnityEngine;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.Unity
{
    /// <summary>
    /// A UnityEngine.LayerMask converter.
    /// </summary>
    public sealed class LayerMaskConverter : ValueConverter<LayerMask, IntNode>
    {
        /* Protected methods. */
        protected override IntNode ConvertValue(LayerMask obj, IConverterScheme scheme) => new(obj);
        protected override LayerMask DeconvertValue(IntNode node, IConverterScheme scheme) => (int)node.Value;
    }
}
#endif