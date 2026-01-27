namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A color serializer node.
    /// </summary>
    public class ColorNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public ColorValue Value { get; set; }

        /* Constructors. */
        public ColorNode() { }

        public ColorNode(ColorValue value)
        {
            Value = value;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"color: {Value}";
        }

        public void Clear()
        {
            Parent = null;
            Value = new ColorValue();
        }
    }
}