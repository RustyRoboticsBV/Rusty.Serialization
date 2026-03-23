using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A converter between objects and nodes.
    /// </summary>
    public abstract class Converter
    {
        /* Public methods. */

        // Conversion.

        /// <summary>
        /// Create a node from an object.
        /// </summary>
        public abstract INode CreateNode(object obj, CreateNodeContext context);

        /// <summary>
        /// Create the child nodes.
        /// </summary>
        public virtual void PopulateNode(INode node, object obj, AssignNodeContext context) { }


        // Deconversion.

        /// <summary>
        /// Convert a node to a type that this converter can use.
        /// </summary>
        public abstract INode ConvertNode(INode node);

        /// <summary>
        /// Collect the runtime types of a node's children.
        /// </summary>
        public virtual void CollectTypes(INode node, CollectTypesContext context) { }

        /// <summary>
        /// Create an object from a node.
        /// </summary>
        public abstract object CreateObject(INode node, CreateObjectContext context);

        /// <summary>
        /// Fix the missing references of the created object.
        /// </summary>
        public virtual object PopulateObject(object obj, INode node, AssignObjectContext context)
        {
            return obj;
        }

        /* Protected methods. */
        protected InvalidCastException CreateNodeTypeError(object obj)
        {
            return new InvalidCastException($"Converter '{GetType().Name}' cannot handle objects of type {obj.GetType().Name}");
        }

        protected InvalidCastException InvalidNodeError(INode node)
        {
            return new InvalidCastException($"Converter '{GetType().Name}' cannot handle nodes of type {node.GetType().Name}");
        }
    }
}