using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A context for collecting node types.
    /// </summary>
    public sealed class CollectTypesContext : Context
    {
        /* Public properties. */
        public TypedTree TypedTree { get; set; }

        /* Constructors. */
        public CollectTypesContext(ObjectCodec codec) : base(codec) { }

        /* Public methods. */
        /// <summary>
        /// Register the type of a node, and collect the types of its child nodes.
        /// </summary>
        public void Collect(INode node, Type runtimeType)
        {
            // Ignore references.
            if (node is RefNode)
                return;

            // Type nodes: override type.
            // TODO: allow for type aliasses.
            if (node is TypeNode type)
                runtimeType = new TypeName(type.Name);

            // Metadata nodes: propagate type to child node.
            if (node is IMetadataNode metadata)
                Collect(metadata.Child, runtimeType);
                
            // Register node/type pair.
            if (!TypedTree.Register(node, runtimeType))
                throw new InvalidOperationException($"Already collected type of node: {node}");

            // Call collect types on the object's converter to handle child nodes.
            Converter converter = Codec.Converters.Get(runtimeType);
            converter.CollectChildNodeTypes(node, this);
        }
    }
}