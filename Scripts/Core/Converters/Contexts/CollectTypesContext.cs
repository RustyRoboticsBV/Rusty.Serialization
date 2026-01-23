using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    public class CollectTypesContext : SubContext
    {
        /* Constructors. */
        public CollectTypesContext(Converters context) : base(context) { }

        /* Public methods. */
        /// <summary>
        /// Register a node's type.
        /// </summary>
        public void CollectTypes(INode node, Type objType)
        {
            // Defer references.
            if (node is RefNode @ref)
            {
                if (!NodeTypeTable.Has(node))
                    NodeTypeTable.DeferRef(@ref);
            }

            // Unwrap metadata nodes.
            else if (node is TypeNode type)
            {
                objType = new TypeName(type.Name);
                CollectTypes(type.Value, objType);
                if (!NodeTypeTable.Has(node))
                    NodeTypeTable.Add(node, NodeTypeTable[type.Value]);
            }

            else if (node is IMetadataNode metadata)
            {
                CollectTypes(metadata.Value, objType);
                if (!NodeTypeTable.Has(node))
                    NodeTypeTable.Add(node, NodeTypeTable[metadata.Value]);
            }

            // Collect child types.
            else
            {
                IConverter converter = Converters.Get(objType);
                converter.CollectTypes(node, this);
                if (!NodeTypeTable.Has(node))
                    NodeTypeTable.Add(node, objType);
            }
        }
    }
}