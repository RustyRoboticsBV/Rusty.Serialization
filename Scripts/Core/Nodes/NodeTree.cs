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

        ~NodeTree()
        {
            Return(Root);
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

        /* Private methods. */
        private static void Return(INode node)
        {
            if (node == null)
                return;

            if (node is IMetadataNode meta)
                Return(meta.Value);
            else if (node is ListNode list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Return(list.GetValueAt(i));
                }
            }
            else if (node is DictNode dict)
            {
                for (int i = 0; i < dict.Count; i++)
                {
                    Return(dict.GetKeyAt(i));
                    Return(dict.GetValueAt(i));
                }
            }
            else if (node is ObjectNode obj)
            {
                for (int i = 0; i < obj.Count; i++)
                {
                    Return(obj.GetNameAt(i));
                    Return(obj.GetValueAt(i));
                }
            }

            NodePool.Return(node);
        }
    }
}