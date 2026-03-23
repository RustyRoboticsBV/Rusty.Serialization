namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A callable serializer node.
    /// </summary>
    public class CallableNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public INode Target { get; set; }
        public IMemberNameNode Name { get; set; }

        /* Constructors. */
        public CallableNode() { }

        public CallableNode(IMemberNameNode name) : this(null, name) { }

        public CallableNode(INode target, IMemberNameNode name)
        {
            Target = target;
            Name = name;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "Callable:\n" + PrintUtility.PrintDelegate(Target, Name);
        }

        public void Clear()
        {
            Parent = null;
            Target?.Clear();
            Target = null;
            Name?.Clear();
            Name = null;
        }
    }
}