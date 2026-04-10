using System;
using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A context for collecting node types.
    /// </summary>
    public sealed class TypedTree
    {
        /* Public properties. */
        public SemanticTree SemanticTree { get; private set; }
        public Dictionary<INode, Type> Types { get; private set; } = new Dictionary<INode, Type>();

        /* Constructors. */
        public TypedTree(SemanticTree semanticTree)
        {
            SemanticTree = semanticTree;
        }

        /* Public methods. */
        /// <summary>
        /// Register a node and its type.
        /// </summary>
        public bool Register(INode node, Type type)
        {
            return Types.TryAdd(node, type);
        }

        /// <summary>
        /// Get the type of the address node pointed to by a reference node.
        /// </summary>
        public Type GetUnderlyingType(RefNode @ref)
        {
            AddressNode address = SemanticTree.GetAddress(@ref);
            return Types[address];
        }

        /* Internal methods. */
        /// <summary>
        /// Resolve reference node types, using the types of their associated address node.
        /// </summary>
        internal void ResolveReferences()
        {
            for (int i = 0; i < SemanticTree.References.Count; i++)
            {
                RefNode @ref = SemanticTree.References[i];
                AddressNode address = SemanticTree.AddressTable[@ref.Address];
                Types.Add(@ref, Types[address]);
            }
        }
    }
}