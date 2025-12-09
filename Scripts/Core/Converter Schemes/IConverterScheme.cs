using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A serialization scheme.
    /// </summary>
    public interface IConverterScheme
    {
        /* Public methods. */
        /// <summary>
        /// Add a converter for some target type.
        /// </summary>
        public void Add(Type targetT, Type converterT, string alias = null);

        /// <summary>
        /// Convert an object to a node tree.
        /// </summary>
        public NodeTree ConvertToTree(object node);
        /// <summary>
        /// Convert an object to an INode hierarchy.
        /// </summary>
        public INode ConvertToNode(object node);

        /// <summary>
        /// Deconvert a node tree to an object.
        /// </summary>
        public T DeconvertTree<T>(NodeTree tree);
        /// <summary>
        /// Deconvert a node tree to an object.
        /// </summary>
        public object DeconvertTree(Type type, NodeTree tree);
        /// <summary>
        /// Deconvert an INode hierarchy to an object.
        /// </summary>
        public T DeconvertNode<T>(INode node);
        /// <summary>
        /// Deconvert an INode hierarchy to an object.
        /// </summary>
        public object DeconvertNode(Type type, INode node);

        /// <summary>
        /// Get the typename of some object.
        /// </summary>
        public TypeName GetTypeName(Type type);

        /// <summary>
        /// Get a type from a type name.
        /// </summary>
        public Type GetTypeFromName(string typeName);
    }
}