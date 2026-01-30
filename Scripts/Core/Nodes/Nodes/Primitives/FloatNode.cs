namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A floating-point number serializer node.
    /// </summary>
    public class FloatNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public FloatValue Value { get; set; }

        /* Constructors. */
        public FloatNode(FloatValue value)
        {
            Value = value;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "float: " + Value;
        }

        public void Clear()
        {
            Parent = null;
            Value = 0f;
        }
    }
}