using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Conversion;

namespace Rusty.Serialization.Conversion.System
{
    /// <summary>
    /// A double converter.
    /// </summary>
    public sealed class DoubleConverter : FloatConverter<double>
    {
        /* Protected methods. */
        protected override FloatValue ToFloat(double obj) => obj;
        protected override double FromFloat(FloatValue obj) => (double)obj;
    }
}