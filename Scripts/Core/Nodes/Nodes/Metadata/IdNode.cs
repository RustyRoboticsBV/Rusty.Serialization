using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An ID serializer node.
    /// </summary>
    public readonly struct IdNode : INode
    {
        /* Fields. */
        private readonly ulong index;
        private readonly INode value;

        /* Public properties. */
        public readonly ulong ID => index;
        public readonly INode Value => value;

        /* Constructors. */
        public IdNode(ulong index, INode value)
        {
            if (index == ulong.MaxValue)
                throw new ArgumentException("Out of IDs!");
            this.index = index;
            this.value = value;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            if (value == null)
                return "index: " + index + " (null)";

            string objStr = value.ToString().Replace("\n", "\n ");
            return "index: " + index + "\n   => " + objStr;
        }
    }
}