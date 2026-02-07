namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A base class for serializer nodes with a value.
    /// </summary>
    public abstract class ValueNode<T> : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public T Name { get; set; }

        /* Constructors. */
        public ValueNode() { }

        public ValueNode(T value)
        {
            Name = value;
        }

        /* Public methods. */
        public virtual void Clear()
        {
            Parent = null;
            Name = default;
        }
    }
}