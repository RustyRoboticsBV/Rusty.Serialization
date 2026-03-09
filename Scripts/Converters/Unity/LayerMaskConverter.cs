#if UNITY_5_3_OR_NEWER
using UnityEngine;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity layer mask converter.
    /// </summary>
    public class LayerMaskConverter : Converter<LayerMask, BitmaskNode>
    {
        /* Protected method. */
        protected override BitmaskNode CreateNode(LayerMask obj, CreateNodeContext context)
        {
            return new BitmaskNode(obj.value);
        }

        protected override LayerMask CreateObject(BitmaskNode node, CreateObjectContext context)
        {
            LayerMask mask = new LayerMask();
            mask.value = node.Value;
            return mask;
        }
    }
}
#endif