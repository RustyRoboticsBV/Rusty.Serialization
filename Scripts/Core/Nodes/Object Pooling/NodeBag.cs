using System;
using System.Collections.Concurrent;

namespace Rusty.Serialization.Core.Nodes
{
    public interface INodeBag
    {
        public INode Rent();
        public void Return(INode node);
        public void Clear();
    }

    public sealed class NodeBag<NodeT> : INodeBag
        where NodeT : INode, new()
    {
        /* Fields. */
        private ConcurrentStack<NodeT> Free { get; set; } = new ConcurrentStack<NodeT>();

        /* Public methods. */
        INode INodeBag.Rent() => Rent();
        void INodeBag.Return(INode node) => Return((NodeT)node);

        /// <summary>
        /// Rent a node from the bag.
        /// </summary>
        public NodeT Rent()
        {
            if (Free.TryPop(out NodeT result))
            {
                result.Clear();
                return result;
            }
            else
                return new NodeT();
        }

        /// <summary>
        /// Return a string builder into the bag.
        /// </summary>
        public void Return(NodeT node)
        {
            if (node != null)
                Free.Push(node);
        }

        /// <summary>
        /// Delete all free nodes. Rented nodes are not deleted.
        /// </summary>
        public void Clear()
        {
            Free.Clear();
        }
    }
}