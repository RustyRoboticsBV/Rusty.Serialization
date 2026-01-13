using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A NaN serializer node.
    /// </summary>
    public class NanNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }

        /* Conversion operators. */
        public static implicit operator NanNode(StringNode node)
        {
            if (node.Value == "nan")
                return new NanNode();
            throw new ArgumentException($"Cannot convert string {node.Value} to nan.");
        }

        /* Public methods. */
        public override string ToString()
        {
            return "nan";
        }

        public void Clear()
        {
            Parent = null;
        }
    }
}