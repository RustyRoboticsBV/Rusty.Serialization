using System.Collections.Specialized;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET BitVector32 converter.
    /// </summary>
    public class BitVector32Converter : Converter<BitVector32, BitmaskNode>
    {
        /* Protected methods. */
        protected override BitmaskNode CreateNode(BitVector32 obj, CreateNodeContext context)
        {
            bool[] bools = new bool[32];
            for (int i = 0; i < 32; i++)
            {
                bools[i] = obj[i];
            }
            return new BitmaskNode(bools);
        }

        protected override BitVector32 CreateObject(BitmaskNode node, CreateObjectContext context)
        {
            BitVector32 vector = new BitVector32();
            for (int i = 0; i < 32 && i < node.Value.value.Length; i++)
            {
                vector[i] = node.Value.value[i];
            }
            return vector;
        }
    }
}
