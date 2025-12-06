using System.Collections.Generic;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An object serializer node.
    /// </summary>
    public class ObjectNode : INode
    {
        /* Public properties. */
        public KeyValuePair<string, INode>[] Members { get; set; }

        /* Constructors. */
        public ObjectNode(KeyValuePair<string, INode>[] members)
        {
            Members = members ?? new KeyValuePair<string, INode>[0];
        }

        /* Public methods. */
        public override string ToString()
        {
            if (Members == null)
                return "object: (null)";

            if (Members.Length == 0)
                return "object: (empty)";

            string str = "object:";
            for (int i = 0; i < Members.Length; i++)
            {
                str += '\n' + PrintUtility.PrintPair(Members[i].Key, Members[i].Value, i == Members.Length - 1);
            }
            return str;
        }

        public void Clear()
        {
            Members = null;
        }

        public void ClearRecursive()
        {
            // Clear child nodes.
            for (int i = 0; i < Members.Length; i++)
            {
                Members[i].Value.ClearRecursive();
            }

            // Clear this node.
            Clear();
        }
    }
}