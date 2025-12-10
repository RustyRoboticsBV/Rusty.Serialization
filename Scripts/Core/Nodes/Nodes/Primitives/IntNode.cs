namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An integer serializer node.
    /// </summary>
    public class IntNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public decimal Value { get; set; }

        /* Constructors. */
        public IntNode(decimal value)
        {
            if (value < 0)
                Value = (long)value;
            else
                Value = (ulong)value;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "int: " + Value;
        }

        public void Clear()
        {
            Parent = null;
            Value = 0;
        }
    }
}