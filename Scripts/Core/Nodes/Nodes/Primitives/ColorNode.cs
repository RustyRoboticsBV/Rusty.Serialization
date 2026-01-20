namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A color serializer node.
    /// </summary>
    public class ColorNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public ColorString Value { get; set; }

        /* Constructors. */
        public ColorNode(ColorString value)
        {
            Value = value;
        }

        /* Conversion operators. */
        public static explicit operator ColorNode(StringNode str)
        {
            return new ColorNode(str.Value);
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"color: #{Value}";
        }

        public void Clear()
        {
            Parent = null;
            Value = (0, 0, 0, 0);
        }
    }
}