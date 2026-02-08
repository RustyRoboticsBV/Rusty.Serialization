namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A time serializer node.
    /// </summary>
    public class TimeNode : ValueNode<TimeValue>
    {
        /* Constructors. */
        public TimeNode() { }

        public TimeNode(TimeValue value) : base(value) { }

        public TimeNode(IntValue year, IntValue month, IntValue day, IntValue hour, IntValue minute, FloatValue second)
        : this(new TimeValue(year, month, day, hour, minute, second)) { }

        /* Public methods. */
        public override string ToString()
        {
            return $"Time: {Value}";
        }
    }
}