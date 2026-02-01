namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A base class for serializer nodes with a value.
    /// </summary>
    public abstract class ValueNode<T> : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public T Value { get; set; }

        /* Constructors. */
        public ValueNode() { }

        public ValueNode(T value)
        {
            Value = value;
        }

        /* Public methods. */
        public virtual void Clear()
        {
            Parent = null;
            Value = default;
        }
    }
}