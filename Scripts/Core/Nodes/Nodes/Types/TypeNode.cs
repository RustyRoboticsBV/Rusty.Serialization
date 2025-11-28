using System;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A type label serializer node.
    /// </summary>
    public readonly struct TypeNode : INode
    {
        /* Fields. */
        private readonly string name;
        private readonly INode value;

        /* Public properties. */
        public readonly string Name => name;
        public readonly INode Value => value;

        /* Constructors. */
        public TypeNode(string name, INode value)
        {
            this.name = name;
            this.value = value;
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            if (value == null)
                return "type: " + name + " (null)";

            string objStr = value.ToString().Replace("\n", "\n ");
            return "type: " + name + "\n   => " + objStr;
        }
    }
}