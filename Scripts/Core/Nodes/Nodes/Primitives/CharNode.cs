namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A character serializer node.
    /// </summary>
    public readonly struct CharNode : INode
    {
        /* Fields. */
        private readonly int value;

        /* Public properties. */
        public readonly int Value => value;

        /* Constructors. */
        public CharNode(int value)
        {
            this.value = value;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            if (value <= char.MaxValue && CharUtility.Check((char)value))
                return "char: " + (char)value;
            else
                return "char: \\u" + HexUtility.ToHexString(value);
        }
    }
}