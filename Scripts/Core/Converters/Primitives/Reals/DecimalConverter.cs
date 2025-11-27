using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A decimal converter.
    /// </summary>
    public sealed class DecimalConverter : ValueConverter<decimal, RealNode>
    {
        /* Protected methods. */
        protected override RealNode Convert(decimal obj, Context context) => new(obj);
        protected override decimal Deconvert(RealNode node, Context context) => node.Value;
    }
}