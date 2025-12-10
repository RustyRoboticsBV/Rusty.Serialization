namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A character serializer node.
    /// </summary>
    public class CharNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public string Value { get; set; } = "\0";

        /* Constructors. */
        public CharNode(string value)
        {
            Value = value;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "char: " + Value;
        }

        public void Clear()
        {
            Parent = null;
            Value = "\0";
        }
    }
}