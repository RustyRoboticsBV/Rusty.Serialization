using System;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// An object serializer node.
    /// </summary>
    public class ObjectNode : ICollectionNode
    {
        /* Public properties. */
        public ITreeElement Parent { get; set; }
        public KeyValuePair<string, INode>[] Members { get; set; }
        public int Count => Members.Length;

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

        /* Conversion operators. */
        public static implicit operator ObjectNode(TimeNode node)
        {
            ObjectNode obj = new ObjectNode(6);
            obj.Members[0] = new KeyValuePair<string, INode>("year", new IntNode(node.Year));
            obj.Members[1] = new KeyValuePair<string, INode>("month", new IntNode(node.Month));
            obj.Members[2] = new KeyValuePair<string, INode>("day", new IntNode(node.Day));
            obj.Members[3] = new KeyValuePair<string, INode>("hour", new IntNode(node.Hour));
            obj.Members[4] = new KeyValuePair<string, INode>("minute", new IntNode(node.Minute));
            obj.Members[5] = new KeyValuePair<string, INode>("second", new IntNode(node.Second));
            return obj;
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

        /// <summary>
        /// Get the identifier at some index.
        /// </summary>
        public string GetIdentifierAt(int index)
        {
            return Members[index].Key;
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