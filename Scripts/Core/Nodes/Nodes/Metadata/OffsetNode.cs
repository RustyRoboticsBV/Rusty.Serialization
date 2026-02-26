namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An offset serializer node.
    /// </summary>
    public class OffsetNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public OffsetValue Offset { get; set; }
        public TimestampNode Time { get; set; }

        /* Constructors. */
        public OffsetNode() : this(default, null) { }

        public OffsetNode(OffsetValue offset, TimestampNode time)
        {
            Offset = offset;
            Time = time;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"Offset: {Offset}\n{PrintUtility.PrintChild(Time)}";
        }

        public void Clear()
        {
            Parent = null;
            Offset = OffsetValue.UTC0;
            Time = null;
        }
    }
}