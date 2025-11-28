namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A color serializer node.
    /// </summary>
    public struct ColorNode : INode
    {
        /* Fields. */
        private readonly byte r;
        private readonly byte g;
        private readonly byte b;
        private readonly byte a;

        /* Public properties. */
        public readonly byte R => r;
        public readonly byte G => g;
        public readonly byte B => b;
        public readonly byte A => a;

        /* Constructors. */
        public ColorNode(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            return $"color: ({r},{g},{b},{a})";
        }
    }
}