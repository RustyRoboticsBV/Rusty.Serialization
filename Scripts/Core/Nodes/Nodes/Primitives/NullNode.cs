namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A null serializer node.
    /// </summary>
    public class NullNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }

        /* Public methods. */
        public override string ToString()
        {
            return "null";
        }

        public void Clear()
        {
            Parent = null;
        }
    }
}