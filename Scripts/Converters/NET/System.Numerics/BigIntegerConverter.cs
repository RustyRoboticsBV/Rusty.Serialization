using System.Numerics;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET big integer converter.
    /// </summary>
    public class BigIntegerConverter : TypedConverter<BigInteger, IntNode>
    {
        /* Protected method. */
        protected override IntNode CreateNode2(BigInteger obj, CreateNodeContext context) => new IntNode(obj);

        protected override BigInteger CreateObject2(IntNode node, CreateObjectContext context) => (BigInteger)node.Value;
    }
}