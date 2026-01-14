using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    public class CollectTypesContext : SubContext
    {
        /* Constructors. */
        public CollectTypesContext(ConversionContext context) : base(context) { }

        /* Public methods. */
        /// <summary>
        /// Register a node's type.
        /// </summary>
        public void CollectTypes(INode node, Type type)
        {
            // Defer references.
            if (node is RefNode @ref)
                NodeTypeTable.DeferRef(@ref);

            // Unwrap metadata nodes.
            else if (node is IMetadataNode metadata)
            {
                CollectTypes(metadata.Value, type);
                NodeTypeTable.Add(node, NodeTypeTable[metadata.Value]);
            }

            // Collect child types.
            else
            {
                UnityEngine.Debug.Log(node);
                IConverter converter = Converters.Get(type);
                converter.CollectTypes(node, this);
                NodeTypeTable.Add(node, type);
            }
        }
    }
}