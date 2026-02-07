using System.Collections;
using Rusty.Serialization.Core.Conversion;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.DotNet
{
    /// <summary>
    /// A .NET bit array converter.
    /// </summary>
    public class BitArrayConverter : Converter<BitArray, BytesNode>
    {
        /* Protected methods. */
        protected override BytesNode CreateNode(BitArray obj, CreateNodeContext context)
        {
            byte[] bytes = new byte[obj.Length];
            obj.CopyTo(bytes, 0);
            return new(bytes);
        }

        protected override BitArray CreateObject(BytesNode node, CreateObjectContext context)
        {
            return new BitArray(node.Name);
        }
    }
}
