using System.Collections;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Converters;

namespace Rusty.Serialization.Converters.System
{
    /// <summary>
    /// A System.Collections.BitArray converter.
    /// </summary>
    public sealed class BitArrayConverter : ReferenceConverter<BitArray, BinaryNode>
    {
        /* Protected methods. */
        protected override BinaryNode CreateNode(BitArray obj, IConverterScheme scheme, SymbolTable table) => new(ToByteArray(obj));
        protected override BitArray CreateObject(BinaryNode node, IConverterScheme scheme, ParsingTable table) => new(node.Value);

        /* Private methods. */
        static byte[] ToByteArray(BitArray obj)
        {
            int numBytes = (obj.Count + 7) / 8;
            byte[] bytes = new byte[numBytes];
            obj.CopyTo(bytes, 0);
            return bytes;
        }
    }
}