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

        /* Public methods. */
        public override string ToString()
        {
            return $"color: {Value}";
        }
    }
}