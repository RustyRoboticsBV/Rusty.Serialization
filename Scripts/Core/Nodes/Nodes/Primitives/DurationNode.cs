namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A duration serializer node.
    /// </summary>
    public sealed class DurationNode : ValueNode<DurationValue>
    {
        /* Constructors. */
        public DurationNode() : base() { }

        public DurationNode(DurationValue value) : base(value) { }

        public DurationNode(BoolValue negative, IntValue days, IntValue hours, IntValue minutes, FloatValue seconds)
            : this(new DurationValue(negative, days, hours, minutes, seconds)) { }
            
        /* Public methods. */
        public override string ToString()
        {
            return $"Duration: {Value}";
        }
    }
}