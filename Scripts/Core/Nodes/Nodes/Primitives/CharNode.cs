namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A character serializer node.
    /// </summary>
    public class CharNode : ValueNode<UnicodePair>
    {
        /* Constructors. */
        public CharNode() : base() { }

        public CharNode(UnicodePair value) : base(value) { }

        /* Public methods. */
        public override string ToString()
        {
            if (Value == '\0')
                return "char: null";
            if (Value == ' ')
                return "char: space";
            if (Value == '\t')
                return "char: horizontal tab";
            if (Value == '\v')
                return "char: vertical tab";
            if (Value == '\n')
                return "char: line feed";
            if (Value == '\f')
                return "char: form feed";
            if (Value == '\r')
                return "char: carriage return";
            if (Value == '\a')
                return "char: alert";
            if (Value == '\b')
                return "char: backspace";
            return "char: " + Value;
        }

        public override void Clear()
        {
            Parent = null;
            Value = '\0';
        }
    }
}