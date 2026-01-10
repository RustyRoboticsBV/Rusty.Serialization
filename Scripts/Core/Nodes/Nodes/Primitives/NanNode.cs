namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A NaN serializer node.
    /// </summary>
    public class NanNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }

        /* Public methods. */
        public override string ToString()
        {
            return "nan";
        }

        public void Clear()
        {
            Parent = null;
        }
    }
}