using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A infinity serializer node.
    /// </summary>
    public class InfinityNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public bool Positive { get; set; }

        /* Constructors. */
        public InfinityNode(bool positive)
        {
            Positive = positive;
        }

        /* Conversion operators. */
        public static implicit operator InfinityNode(StringNode node)
        {
            if (node.Value == "inf")
                return new InfinityNode(true);
            if (node.Value == "-inf")
                return new InfinityNode(false);
            throw new ArgumentException($"Cannot convert string {node.Value} to infinity.");
        }

        /* Public methods. */
        public override string ToString()
        {
            if (Positive)
                return "infinity: positive";
            else
                return "infinity: negative";
        }

        public void Clear()
        {
            Parent = null;
            Positive = true;
        }
    }
}