using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A System.Half converter.
    /// </summary>
    public sealed class HalfConverter : ValueConverter<Half, RealNode>
    {
        /* Protected methods. */
        protected override RealNode Convert(Half obj, Context context) => new((decimal)obj);
        protected override Half Deconvert(RealNode node, Context context) => (Half)node.Value;
    }
}