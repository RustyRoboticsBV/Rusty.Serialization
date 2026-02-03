using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A bytes serializer node.
    /// </summary>
    public class BytesNode : ValueNode<byte[]>
    {
        /* Constructors. */
        public BytesNode() : base() { }

        public BytesNode(byte[] value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            return $"bytes: {Convert.ToBase64String(Value)}";
        }
    }
}