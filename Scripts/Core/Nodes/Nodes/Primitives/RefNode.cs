namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A reference serializer node.
    /// </summary>
    public class RefNode : INode
    {
        /* Public properties. */
        public ulong ID { get; set; }

        /* Constructors. */
        public RefNode(ulong id)
        {
            ID = id;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "ref: " + ID;
        }

        public void Clear()
        {
            ID = 0;
        }

        public void ClearRecursive() => Clear();
    }
}