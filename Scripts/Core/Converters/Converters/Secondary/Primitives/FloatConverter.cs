using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A float converter.
    /// </summary>
    public sealed class FloatConverter : RealConverter<float>
    {
        /* Protected properties. */
        protected override float NaN => float.NaN;
        protected override float PositiveInfinity => float.PositiveInfinity;
        protected override float NegativeInfinity => float.NegativeInfinity;

        /* Protected methods. */
        protected override FloatNode CreateNode(float obj, CreateNodeContext context) => new(obj);
        protected override float CreateObject(FloatNode node, CreateObjectContext context) => float.Parse(node.Value);

        protected override bool IsNaN(ref float value) => float.IsNaN(value);
        protected override bool IsPositiveInfinity(ref float value) => float.IsPositiveInfinity(value);
        protected override bool IsNegativeInfinity(ref float value) => float.IsNegativeInfinity(value);
    }
}