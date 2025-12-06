namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A null serializer node.
    /// </summary>
    public class NullNode : INode
    {
        /* Public methods. */
        public override string ToString()
        {
            return "null";
        }

        public void Clear() { }

        public void ClearRecursive() => Clear();
    }
}