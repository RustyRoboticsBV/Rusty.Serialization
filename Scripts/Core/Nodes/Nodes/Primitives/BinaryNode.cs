using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A binary string serializer node.
    /// </summary>
    public readonly struct BinaryNode : INode
    {
        /* Fields. */
        private readonly byte[] value;

        /* Public properties. */
        public readonly byte[] Value => value;

        /* Constructors. */
        public BinaryNode(byte[] value)
        {
            this.value = value;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            return $"binary: 0x{HexUtility.ToHexString(value)}";
        }
    }
}