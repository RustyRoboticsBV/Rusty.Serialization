using System.Collections.Generic;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An object serializer node.
    /// </summary>
    public readonly struct ObjectNode : INode
    {
        /* Fields. */
        private readonly KeyValuePair<string, INode>[] members;

        /* Public properties. */
        public readonly KeyValuePair<string, INode>[] Members => members;

        /* Constructors. */
        public ObjectNode(KeyValuePair<string, INode>[] members)
        {
            this.members = members ?? [];
        }

        /* Public methods. */
        public override readonly string ToString()
        {
            string header = $"object:";

            string str = header;
            foreach (var member in members)
            {
                string valStr = member.Value.ToString().Replace("\n", "\n ");
                str += $"\n- {member.Key}: {valStr}";
            }
            return str;
        }
    }
}