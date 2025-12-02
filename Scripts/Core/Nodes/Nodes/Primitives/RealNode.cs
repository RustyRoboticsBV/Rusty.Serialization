namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A real number serializer node.
    /// </summary>
    public readonly struct RealNode : INode
    {
        /* Fields. */
        private readonly PeterO.Numbers.EDecimal value;

        /* Public properties. */
        public readonly PeterO.Numbers.EDecimal Value => value;

        /* Constructors. */
        public RealNode(PeterO.Numbers.EDecimal value)
        {
            this.value = value;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            return "real: " + value;
        }
    }
}