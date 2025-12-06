namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A character serializer node.
    /// </summary>
    public class CharNode : INode
    {
        /* Public properties. */
        public INode Parent { get; set; }
        public int Value { get; set; }

        /* Constructors. */
        public CharNode(int value)
        {
            Value = value;
        }

        /* Public methods. */
        public override string ToString()
        {
            if (Value <= char.MaxValue && char.IsControl((char)Value))
                return "char: " + (char)Value;
            else
                return "char: \\u" + HexUtility.ToHexString(Value);
        }

        public void Clear()
        {
            Parent = null;
            Value = 0;
        }
    }
}