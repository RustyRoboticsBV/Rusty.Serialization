using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An offset serializer node.
    /// </summary>
    public class OffsetNode : ValueNode<OffsetValue>, IMetadataNode
    {
        /* Public properties. */
        public TimestampNode Child { get; set; }
        INode IMetadataNode.Child => Child;

        /* Constructors. */
        public OffsetNode(OffsetValue offset, TimestampNode time)
        {
            Value = offset;
            Child = time;
        }

        /* Public methods. */
        public override string ToString()
        {
            return $"Offset: {Value}\n{PrintUtility.PrintChild(Child)}";
        }

        public override void Clear()
        {
            base.Clear();
            Value = OffsetValue.UTC0;
            Child?.Clear();
            Child = null;
        }

        void IContainerNode.ReplaceChild(INode oldChild, INode newChild)
            => ReplaceChild(oldChild as TimestampNode, newChild as TimestampNode);
        public void ReplaceChild(TimestampNode oldChild, TimestampNode newChild)
        {
            if (Child == oldChild)
            {
                if (oldChild.Parent == this)
                    oldChild.Parent = null;
                newChild.Parent = this;
                Child = newChild;
                return;
            }
            throw new ArgumentException($"'{oldChild}' was not a child of '{this}'.");
        }
    }
}