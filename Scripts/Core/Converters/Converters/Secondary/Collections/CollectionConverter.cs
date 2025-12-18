using Rusty.Serialization.Core.Nodes;
using System.Collections;
using System.Collections.Generic;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An ICollection converter.
    /// </summary>
    public abstract class CollectionConverter<CollectionT, ElementT, NodeT> : EnumerableConverter<CollectionT, ElementT, NodeT>
        where CollectionT : ICollection<ElementT>
        where NodeT : ICollectionNode
    {
        /* Protected methods. */
        protected override int GetCount(CollectionT obj) => obj.Count;
    }
}