namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A color serializer node.
    /// </summary>
    public class ColorNode : ValueNode<ColorValue>
    {
        /* Constructors. */
        public ColorNode() : base() { }

        public ColorNode(ColorValue value) : base(value) { }

        public ColorNode(byte r, byte g, byte b, byte a) : this(new ColorValue(r, g, b, a)) { }

        /* Public methods. */
        public override string ToString()
        {
            return $"color: {Value}";
        }
    }
}