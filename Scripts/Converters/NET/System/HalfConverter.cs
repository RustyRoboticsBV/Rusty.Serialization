#if NET5_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET half converter.
    /// </summary>
    public class HalfConverter : RealConverter<Half>
    {
        /* Protected properties. */
        protected override Half NaN => Half.NaN;
        protected override Half PositiveInfinity => Half.PositiveInfinity;
        protected override Half NegativeInfinity => Half.NegativeInfinity;
        protected override Half Pi => (Half)Math.PI;
        protected override Half E => (Half)Math.E;

        /* Protected methods. */
        protected override FloatNode CreateNode(Half obj, CreateNodeContext context) => new FloatNode((float)obj);
        protected override Half CreateObject(FloatNode node, CreateObjectContext context) => (Half)(float)node.Value;

        protected override bool IsNaN(ref Half value) => Half.IsNaN(value);
        protected override bool IsPositiveInfinity(ref Half value) => Half.IsPositiveInfinity(value);
        protected override bool IsNegativeInfinity(ref Half value) => Half.IsNegativeInfinity(value);
        protected override bool IsPi(ref Half value) => Math.Abs((float)value - MathF.PI) < 1e-3;
        protected override bool IsE(ref Half value) => Math.Abs((float)value - MathF.E) < 1e-3;
    }
}
#endif