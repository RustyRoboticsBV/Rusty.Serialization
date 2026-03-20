namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A timestamp serializer node.
    /// </summary>
    public sealed class TimestampNode : ValueNode<TimestampValue>
    {
        /* Constructors. */
        public TimestampNode() : base() { }

        public TimestampNode(TimestampValue value) : base(value) { }

        public TimestampNode(IntValue year, IntValue month, IntValue day, IntValue hour, IntValue minute, FloatValue second)
        : this(new TimestampValue(year, month, day, hour, minute, second)) { }

        /* Public methods. */
        public override string ToString()
        {
            return $"Timestamp: {Value}";
        }
    }
}