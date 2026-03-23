namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A uid serializer node.
    /// </summary>
    public sealed class UidNode : ValueNode<UidValue>
    {
        /* Constructors. */
        public UidNode() : this(UidValue.Empty) { }

        public UidNode(UidValue value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            return "uid: " + Value;
        }
    }
}