namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A base class for serializer nodes with a value.
    /// </summary>
    public abstract class ValueNode<T> : LeafNode
    {
        /* Public properties. */
        public T Value { get; set; }

        /* Constructors. */
        public ValueNode() { }

        public ValueNode(T value)
        {
            Value = value;
        }

        /* Public methods. */
        public override void Clear()
        {
            base.Clear();
            Value = default;
        }
    }
}