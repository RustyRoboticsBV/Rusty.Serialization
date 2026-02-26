using System;
using System.Collections.Generic;

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
        private List<NodeT> Free { get; set; } = new(1);
        private List<NodeT> Rented { get; } = new();

        private readonly object @lock = new object();

        /* Public methods. */
        INode INodeBag.Rent() => Rent();
        void INodeBag.Return(INode node) => Return((NodeT)node);

        /// <summary>
        /// Rent a node from the bag.
        /// </summary>
        public NodeT Rent()
        {
            lock (@lock)
            {
                if (Free.Count == 0)
                {
                    NodeT node = new NodeT();
                    Rented.Add(node);
                    return node;
                }
                else
                {
                    NodeT renting = Free[Free.Count - 1];
                    renting.Clear();
                    Free.RemoveAt(Free.Count - 1);
                    Rented.Add(renting);
                    return renting;
                }
            }
        }

        /// <summary>
        /// Return a string builder into the bag.
        /// </summary>
        public void Return(NodeT node)
        {
            lock (@lock)
            {
                if (!Rented.Remove(node))
                    throw new ArgumentException($"The node {node} was not rented from this bag.");
                Free.Add(node);
            }
        }

        /// <summary>
        /// Delete all free nodes. Rented nodes are not deleted.
        /// </summary>
        public void Clear()
        {
            lock (@lock)
            {
                Free = new List<NodeT>();
            }
        }
    }
}