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
        public INode Parent { get; set; }
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

        public void WrapChild(INode child, INode wrapper)
        {
            if (wrapper is IdNode id)
            {
                id.Value = child;
                child.Parent = id;
                for (int i = 0; i < Members.Length; i++)
                {
                    if (Members[i].Value == child)
                        Members[i] = new(Members[i].Key, id);
                }
            }
            else
                throw new ArgumentException("We only allow child wrapping for ID nodes.");
        }
    }
}