using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A bytes serializer node.
    /// </summary>
    public sealed class BytesNode : ValueNode<BytesValue>
    {
        /* Constructors. */
        public BytesNode() : this(Array.Empty<byte>()) { }

        public BytesNode(BytesValue value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            return $"bytes: {Value}";
        }
    }
}