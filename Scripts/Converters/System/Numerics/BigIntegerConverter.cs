using System.Numerics;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters
{
    /// <summary>
    /// A System.Numerics.BigInteger converter.
    /// </summary>
    public sealed class BigIntegerConverter : ValueConverter<BigInteger, IntNode>
    {
        /* Protected methods. */
        protected override IntNode Convert(BigInteger obj, Context context) => new((decimal)obj);
        protected override BigInteger Deconvert(IntNode node, Context context) => new(node.Value);
    }
}