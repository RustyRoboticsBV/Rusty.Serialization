using System;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A node tree that represents an object hierarchy.
    /// </summary>
    public sealed class NodeTree : ICollectionNode
    {
        /* Public properties. */
        public INode Parent { get; set; }
        public INode Root { get; set; }

        /* Private properties. */
        private Dictionary<string, IdNode> IdLookup { get; set; }

        /* Constructors. */
        public NodeTree(INode root)
        {
            Root = root;
            root.Parent = this;
        }

        /* Public methods. */
        public override string ToString()
        {
            return "TREE:\n" + PrintUtility.PrintChild(Root);
        }

        public void WrapChild(INode child, INode wrapper)
        {
            if (wrapper is IdNode id)
            {
                id.Value = child;
                child.Parent = id;
                Root = id;
            }
            else
                throw new ArgumentException("We only allow child wrapping for ID nodes.");
        }

        public void Clear()
        {
            Root?.Clear();
            Root = null;
            Parent = null;
        }

        /// <summary>
        /// Find an ID node corresponding to some ID.
        /// </summary>
        public IdNode FindId(string id)
        {
            if (IdLookup == null)
            {
                IdLookup = new();
                FindAll(IdLookup, Root);
            }

            return IdLookup[id];
        }

        private void FindAll(Dictionary<string, IdNode> found, INode root)
        {
            if (root is IdNode id)
            {
                found[id.Name] = id;
                FindAll(found, id.Value);
            }
            else if (root is TypeNode type)
                FindAll(found, type.Value);
            else if (root is ListNode list)
            {
                for (int i = 0; i < list.Elements.Length; i++)
                {
                    FindAll(found, list.Elements[i]);
                }
            }
            else if (root is DictNode dict)
            {
                for (int i = 0; i < dict.Pairs.Length; i++)
                {
                    FindAll(found, dict.Pairs[i].Key);
                    FindAll(found, dict.Pairs[i].Value);
                }
            }
            else if (root is ObjectNode obj)
            {
                for (int i = 0; i < obj.Members.Length; i++)
                {
                    FindAll(found, obj.Members[i].Value);
                }
            }
        }
    }
}