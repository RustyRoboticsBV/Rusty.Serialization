using Rusty.Serialization.Core.Nodes;
using System;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A serialization scheme.
    /// </summary>
    public interface IConverterScheme
    {
        /* Public methods. */
        /// <summary>
        /// Convert an object to an INode hierarchy.
        /// </summary>
        public INode Convert(object node);
        /// <summary>
        /// Convert an object to an INode hierarchy.
        /// </summary>
        public INode Convert<T>(T node);
        /// <summary>
        /// Convert an object to an INode hierarchy.
        /// </summary>
        public INode Convert<T>(ref T node);

        /// <summary>
        /// Deconvert an INode hierarchy to an object.
        /// </summary>
        public T Deconvert<T>(INode node);
        /// <summary>
        /// Deconvert an INode hierarchy to an object.
        /// </summary>
        public object Deconvert(Type type, INode node);

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