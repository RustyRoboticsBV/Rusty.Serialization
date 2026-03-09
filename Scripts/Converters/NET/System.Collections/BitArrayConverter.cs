using System.Collections;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET bit array converter.
    /// </summary>
    public class BitArrayConverter : Converter<BitArray, BitmaskNode>
    {
        /* Protected methods. */
        protected override BitmaskNode CreateNode(BitArray obj, CreateNodeContext context)
        {
            bool[] bools = new bool[obj.Length];
            for (int i = 0; i < obj.Length; i++)
            {
                bools[i] = obj[i];
            }
            return new BitmaskNode(bools);
        }

        protected override BitArray CreateObject(BitmaskNode node, CreateObjectContext context)
        {
            return new BitArray((bool[])node.Value);
        }
    }
}
