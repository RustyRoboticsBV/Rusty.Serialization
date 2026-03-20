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

        /// <summary>
        /// Get a lookup of all address nodes in this tree.
        /// </summary>
        public Dictionary<string, AddressNode> GetAddressNodes()
        {
            if (AddressLookup == null)
            {
                AddressLookup = new Dictionary<string, AddressNode>();
                CollectReferences(Root);
            }
            return AddressLookup;
        }

        /* Private methods. */
        private void CollectReferences(INode node)
        {
            if (node is AddressNode address)
                AddressLookup[address.Name] = address;

            if (node is IMetadataNode metadata)
                CollectReferences(metadata);
            else if (node is ListNode list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    CollectReferences(list.GetValueAt(i));
                }
            }
            else if (node is DictNode dict)
            {
                for (int i = 0; i < dict.Count; i++)
                {
                    CollectReferences(dict.GetKeyAt(i));
                    CollectReferences(dict.GetValueAt(i));
                }
            }
            else if (node is ObjectNode obj)
            {
                for (int i = 0; i < obj.Count; i++)
                {
                    CollectReferences(obj.GetValueAt(i));
                }
            }
            else if (node is CallableNode func)
            {
                CollectReferences(func.Target);
            }
        }
    }
}