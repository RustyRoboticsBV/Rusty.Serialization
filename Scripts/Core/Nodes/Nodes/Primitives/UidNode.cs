using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A uid serializer node.
    /// </summary>
    public class UidNode : ValueNode<Guid>
    {
        /* Constructors. */
        public UidNode() : this(Guid.Empty) { }

        public UidNode(Guid value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            return "uid: " + Value;
        }
    }
}