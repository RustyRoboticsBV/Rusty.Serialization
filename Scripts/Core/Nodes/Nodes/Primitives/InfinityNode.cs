namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An infinity serializer node.
    /// </summary>
    public sealed class InfinityNode : ValueNode<InfinityValue>
    {
        /* Constructors. */
        public InfinityNode() : this(true) { }

        public InfinityNode(InfinityValue value) : base(value) { }

        /* Public methods. */
        public override string ToString() => $"infinity: {Value}";
    }
}