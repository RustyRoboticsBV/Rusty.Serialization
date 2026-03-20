using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A bitmask serializer node.
    /// </summary>
    public sealed class BitmaskNode : ValueNode<BitmaskValue>
    {
        /* Constructors. */
        public BitmaskNode() : this(Array.Empty<bool>()) { }

        public BitmaskNode(BitmaskValue value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            return "bitmask: " + Value;
        }
    }
}