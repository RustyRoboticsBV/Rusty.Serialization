using System;
using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// An ICollection converter.
    /// </summary>
    public abstract class CollectionConverter<CollectionT, ElementT, NodeT> : EnumerableConverter<CollectionT, ElementT, NodeT>
        where CollectionT : ICollection<ElementT>
        where NodeT : ICollectionNode
    {
        /* Protected methods. */
        protected override CollectionT AssignObject(CollectionT obj, NodeT node, AssignObjectContext context)
        {
            Type elementType = typeof(ElementT);
            for (int i = 0; i < node.Count; i++)
            {
                ElementT element = context.CreateChildObject<ElementT>(node.GetValueAt(i));
                obj.Add(element);
            }
            return obj;
        }

        protected override int GetCount(CollectionT obj) => obj.Count;
    }
}