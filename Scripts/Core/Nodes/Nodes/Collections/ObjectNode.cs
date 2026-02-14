using System;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Nodes
{
    using Member = KeyValuePair<IMemberNameNode, INode>;

    /// <summary>
    /// An object serializer node.
    /// </summary>
    public class ObjectNode : ICollectionNode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public List<Member> Members { get; set; }
        public int Count => Members.Count;

        /* Constructors. */
        public ObjectNode() : this(new List<Member>()) { }

        public ObjectNode(int capacity) : this(new Member[capacity]) { }

        public ObjectNode(params Member[] members) : this(new List<Member>(members)) { }

        public ObjectNode(List<Member> members)
        {
            Members = members ?? new List<Member>();

            for (int i = 0; i < Members.Count; i++)
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

            if (Members.Count == 0)
                return "object: (empty)";

            string str = "object:";
            for (int i = 0; i < Members.Count; i++)
            {
                str += '\n' + PrintUtility.PrintPair(Members[i].Key, Members[i].Value, i == Members.Count - 1);
            }
            return str;
        }

        public void Clear()
        {
            Parent = null;
            Members = null;
            for (int i = 0; i < Members.Count; i++)
            {
                Members[i].Value.Clear();
            }
        }

        /// <summary>
        /// Get the index of a member node. Returns -1 if the node is not a member.
        /// </summary>
        public int IndexOf(INode member)
        {
            for (int i = 0; i < Members.Count; i++)
            {
                if (Members[i].Value == member)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Get the identifier at some index.
        /// </summary>
        public IMemberNameNode GetNameAt(int index)
        {
            return Members[index].Key;
        }

        /// <summary>
        /// Add a new member.
        /// </summary>
        public void AddMember(IMemberNameNode node, INode value)
        {
            if (node is SymbolNode || node is ScopeNode)
                Members.Add(new Member(node, value));
        }

        public INode GetValueAt(int index)
        {
            return Members[index].Value;
        }

        public void ReplaceChild(INode oldChild, INode newChild)
        {
            int index = IndexOf(oldChild);
            if (index != -1)
            {
                if (oldChild.Parent == this)
                    oldChild.Parent = null;
                newChild.Parent = this;
                Members[index] = new(Members[index].Key, newChild);
                return;
            }
            throw new ArgumentException($"'{oldChild}' was not a child of '{this}'.");
        }
    }
}