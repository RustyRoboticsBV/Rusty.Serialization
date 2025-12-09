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
        public INode Root { get; private set; }

        /* Private properties. */
        private Dictionary<string, IdNode> IdLookup { get; set; }

        /* Constructors. */
        public NodeTree() { }

        public NodeTree(INode root)
        {
            SetRoot(root);
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
        /// Set the root of this tree.
        /// </summary>
        public void SetRoot(INode node)
        {
            node.Parent = this;
            Root = node;
        }
    }
}