using Rusty.Serialization.Core.Contexts;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A double converter.
    /// </summary>
    public sealed class DoubleConverter : ValueConverter<double, RealNode>
    {
        /* Protected methods. */
        protected override RealNode Convert(double obj, Context context) => new((decimal)obj);
        protected override double Deconvert(RealNode node, Context context) => (double)node.Value;
    }
}