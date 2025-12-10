using System.Collections.Generic;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An object serializer node.
    /// </summary>
    public class ObjectNode : INode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public KeyValuePair<string, INode>[] Members { get; set; }

        /* Constructors. */
        public ObjectNode(int capacity) : this(new KeyValuePair<string, INode>[capacity]) { }

        public ObjectNode(KeyValuePair<string, INode>[] members)
        {
            Members = members ?? new KeyValuePair<string, INode>[0];

            for (int i = 0; i < Members.Length; i++)
            {
                if (Members[i].Value != null)
                    Members[i].Value.Parent = this;
            }
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
            Parent = null;
            Members = null;
            for (int i = 0; i < Members.Length; i++)
            {
                Members[i].Value.Clear();
            }
        }

        /// <summary>
        /// Get the index of a member node. Returns -1 if the node is not a member.
        /// </summary>
        public int IndexOf(INode member)
        {
            for (int i = 0; i < Members.Length; i++)
            {
                if (Members[i].Value == member)
                    return i;
            }
            return -1;
        }
    }
}