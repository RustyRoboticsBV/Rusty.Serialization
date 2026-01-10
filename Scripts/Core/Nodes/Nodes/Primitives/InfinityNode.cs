namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A infinity serializer node.
    /// </summary>
    public class InfinityNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public bool Positive { get; set; }

        /* Constructors. */
        public InfinityNode(bool positive)
        {
            Positive = positive;
        }

        /* Public methods. */
        public override string ToString()
        {
            if (Positive)
                return "infinity: positive";
            else
                return "infinity: negative";
        }

        public void Clear()
        {
            Parent = null;
            Positive = true;
        }
    }
}