using System;
using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A registry for the C# types of compiler nodes, used during parsing.
    /// </summary>
    public class NodeTypeTable
    {
        /* Private properties. */
        private Dictionary<INode, Type> NodeTypes { get; } = new();
        private Dictionary<string, IdNode> Ids { get; } = new();
        private List<RefNode> Refs { get; } = new();

        /* Indexers. */
        /// <summary>
        /// Get the type of a node.
        /// </summary>
        public Type this[INode node] => GetType(node);

        /* Public methods. */
        public override string ToString()
        {
            string str = "";
            foreach (INode node in NodeTypes.Keys)
            {
                if (str != "")
                    str += '\n';
                str += "[" + NodeTypes[node] + "] " + node;
            }
            return str;
        }

        /// <summary>
        /// Add a node-type pair.
        /// </summary>
        public void Add(INode node, Type type)
        {
            if (node is RefNode @ref)
                DeferRef(@ref);
            else if (node is IdNode id)
            {
                Ids.Add(id.Name, id);
                NodeTypes[node] = type;
            }
            else
                NodeTypes[node] = type;
        }

        /// <summary>
        /// Add a reference node.
        /// </summary>
        public void DeferRef(RefNode @ref)
        {
            Refs.Add(@ref);
        }

        /// <summary>
        /// Check if a node is present.
        /// </summary>
        public bool Has(INode node)
        {
            return NodeTypes.ContainsKey(node) || (node is RefNode @ref && Refs.Contains(@ref));
        }

        /// <summary>
        /// Get the type of a node.
        /// </summary>
        public Type GetType(INode node)
        {
            if (node is IMetadataNode meta)
                return NodeTypes[meta.Value];
            return NodeTypes[node];
        }

        /// <summary>
        /// Resolve deferred reference nodes.
        /// </summary>
        public void ResolveRefs()
        {
            foreach (RefNode @ref in Refs)
            {
                IdNode id = Ids[@ref.ID];
                Type type = NodeTypes[id];
                NodeTypes[@ref] = type;
            }
            Refs.Clear();
        }

        /// <summary>
        /// Get an ID node.
        /// </summary>
        public IdNode GetId(RefNode @ref)
        {
            return Ids[@ref.ID];
        }

        /// <summary>
        /// Validate that all nodes in a tree have been collected.
        /// </summary>
        public bool Validate(NodeTree tree)
        {
            if (tree.Root == null)
                return true;
            return Validate(tree.Root);
        }

        /// <summary>
        /// Clear the node type table.
        /// </summary>
        public void Clear()
        {
            NodeTypes.Clear();
            Ids.Clear();
            Refs.Clear();
        }

        /* Private methods. */
        private bool Validate(INode node)
        {
            if (!NodeTypes.ContainsKey(node))
                return false;

            if (node is IMetadataNode metadata)
                return Validate(metadata.Value);

            else if (node is ListNode list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (!Validate(list.GetValueAt(i)))
                        return false;
                }
            }

            else if (node is DictNode dict)
            {
                for (int i = 0; i < dict.Count; i++)
                {
                    if (!Validate(dict.GetKeyAt(i)))
                        return false;
                    if (!Validate(dict.GetValueAt(i)))
                        return false;
                }
            }

            else if (node is ObjectNode obj)
            {
                for (int i = 0; i < obj.Count; i++)
                {
                    if (!Validate(obj.GetValueAt(i)))
                        return false;
                }
            }

            return true;
        }
    }
}