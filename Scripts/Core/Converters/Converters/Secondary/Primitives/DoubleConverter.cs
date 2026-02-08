using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A double converter.
    /// </summary>
    public sealed class DoubleConverter : RealConverter<double>
    {
        /* Protected properties. */
        protected override double NaN => double.NaN;
        protected override double PositiveInfinity => double.PositiveInfinity;
        protected override double NegativeInfinity => double.NegativeInfinity;
        protected override double Pi => Math.PI;
        protected override double E => Math.E;

        /* Protected methods. */
        protected override FloatNode CreateNode(double obj, CreateNodeContext context) => new(obj);
        protected override double CreateObject(FloatNode node, CreateObjectContext context) => (double)node.Value;

        protected override bool IsNaN(ref double value) => double.IsNaN(value);
        protected override bool IsPositiveInfinity(ref double value) => double.IsPositiveInfinity(value);
        protected override bool IsNegativeInfinity(ref double value) => double.IsNegativeInfinity(value);
        protected override bool IsPi(ref double value) => Math.Abs(value - Math.PI) < 1e-15;
        protected override bool IsE(ref double value) => Math.Abs(value - Math.E) < 1e-15;
    }
}