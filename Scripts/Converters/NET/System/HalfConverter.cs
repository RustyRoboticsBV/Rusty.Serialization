#if NET5_0_OR_GREATER
using System;
using Rusty.Serialization.Core.Converters;
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

        /* Protected methods. */
        protected override RealNode CreateNode(Half obj, CreateNodeContext context) => new(obj);
        protected override Half CreateObject(RealNode node, CreateObjectContext context) => Half.Parse(node.Value);

        protected override bool IsNaN(ref Half value) => Half.IsNaN(value);
        protected override bool IsPositiveInfinity(ref Half value) => Half.IsPositiveInfinity(value);
        protected override bool IsNegativeInfinity(ref Half value) => Half.IsNegativeInfinity(value);
    }
}
#endif