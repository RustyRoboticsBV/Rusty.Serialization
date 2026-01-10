namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A character serializer node.
    /// </summary>
    public class CharNode : ITextNode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public CharString Value { get; set; } = '\0';

        string ITextNode.Value
        {
            get => Value;
            set => Value = value;
        }

        /* Constructors. */
        public CharNode() { }

        public CharNode(string value)
        {
            Value = value;
        }

        /* Conversion operators. */
        public static explicit operator CharNode(StringNode str)
        {
            return new CharNode(str.Value);
        }

        /* Public methods. */
        public override string ToString()
        {
            return "char: " + Value;
        }

        public void Clear()
        {
            Parent = null;
            Value = '\0';
        }
    }
}