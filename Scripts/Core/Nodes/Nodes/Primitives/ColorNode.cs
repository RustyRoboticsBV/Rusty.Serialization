namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A color serializer node.
    /// </summary>
    public class ColorNode : INode
    {
        /* Public properties. */
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        /* Constructors. */
        public ColorNode(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"color: ({R},{G},{B},{A})";
        }

        public void Clear()
        {
            R = 0;
            G = 0;
            B = 0;
            A = 0;
        }

        public void ClearRecursive() => Clear();
    }
}