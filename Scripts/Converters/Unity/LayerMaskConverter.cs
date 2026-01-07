#if UNITY_5_3_OR_NEWER
using UnityEngine;
using Rusty.Serialization.Core.Converters;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Unity
{
    /// <summary>
    /// A Unity layer mask converter.
    /// </summary>
    public class LayerMaskConverter : Converter<LayerMask, IntNode>
    {
        /* Protected method. */
        protected override IntNode CreateNode(LayerMask obj, CreateNodeContext context)
        {
            return new IntNode(obj.value);
        }

        protected override LayerMask CreateObject(IntNode node, CreateObjectContext context)
        {
            return LayerMask.GetMask(node.Value);
        }
    }
}
#endif