using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A float converter.
    /// </summary>
    public sealed class FloatConverter : ValueConverter<float, RealNode>
    {
        /* Protected methods. */
        protected override RealNode Convert(float obj, Context context) => new((decimal)obj);
        protected override float Deconvert(RealNode node, Context context) => (float)node.Value;
    }
}