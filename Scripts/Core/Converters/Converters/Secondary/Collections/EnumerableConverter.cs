using System;
using System.Collections.Generic;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An IEnumerable converter.
    /// </summary>
    public abstract class EnumerableConverter<EnumerableT, ElementT, NodeT> : CompositeConverter<EnumerableT, NodeT>
        where EnumerableT : IEnumerable<ElementT>
        where NodeT : ICollectionNode
    {
        /* Protected methods. */
        protected override NodeT CreateNode(EnumerableT obj, CreateNodeContext context)
        {
            return (NodeT)Activator.CreateInstance(typeof(NodeT), GetCount(obj));
        }

        protected override EnumerableT CreateObject(NodeT node, CreateObjectContext context)
            => (EnumerableT)Activator.CreateInstance(typeof(EnumerableT));

        /// <summary>
        /// Get the number of elements in a collection.
        /// </summary>
        protected virtual int GetCount(EnumerableT obj)
        {
            int count = 0;
            foreach (ElementT element in obj)
            {
                count++;
            }
            return count;
        }
    }
}