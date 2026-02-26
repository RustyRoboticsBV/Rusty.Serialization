using System.Collections.Generic;

namespace Rusty.Serialization.Core.Nodes
{
    /// <summary>
    /// A node tree that represents an object hierarchy.
    /// </summary>
    public sealed class NodeTree : ITreeElement
    {
        /* Public properties. */
        public INode Root { get; private set; }

        /* Private properties. */
        private Dictionary<string, AddressNode> AddressLookup { get; set; }

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

        public void Clear()
        {
            Root?.Clear();
            Root = null;
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