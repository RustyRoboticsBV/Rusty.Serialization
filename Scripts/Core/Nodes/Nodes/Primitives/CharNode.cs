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
            if (Name == '\0')
                return "char: null";
            if (Name == ' ')
                return "char: space";
            if (Name == '\t')
                return "char: horizontal tab";
            if (Name == '\v')
                return "char: vertical tab";
            if (Name == '\n')
                return "char: line feed";
            if (Name == '\f')
                return "char: form feed";
            if (Name == '\r')
                return "char: carriage return";
            if (Name == '\a')
                return "char: alert";
            if (Name == '\b')
                return "char: backspace";
            return "char: " + Name;
        }
    }
}