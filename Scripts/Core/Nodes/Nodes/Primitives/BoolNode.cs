namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A boolean serializer node.
    /// </summary>
    public class BoolNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public bool Value { get; set; }

        /* Constructors. */
        public BoolNode(bool value)
        {
            Value = value;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "bool: " + Value;
        }

        public void Clear()
        {
            Parent = null;
            Value = false;
        }
    }
}