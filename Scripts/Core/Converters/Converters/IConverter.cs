using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A converter between objects and nodes.
    /// </summary>
    public interface IConverter
    {
        /* Public methods. */
        /// <summary>
        /// Collect the type of a node, as well as the types of members.
        /// </summary>
        public void CollectTypes(INode node, CollectTypesContext context);

        /// <summary>
        /// Create a node from an object.
        /// </summary>
        public INode CreateNode(object obj, CreateNodeContext context);

        /// <summary>
        /// Create an object from a node.
        /// </summary>
        public object CreateObject(INode node, CreateObjectContext context);
    }
}