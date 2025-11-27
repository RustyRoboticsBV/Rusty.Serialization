using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters
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