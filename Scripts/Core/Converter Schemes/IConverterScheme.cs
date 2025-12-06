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
        /// Convert an object to an INode hierarchy.
        /// </summary>
        public INode Convert(object node);

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

        /// <summary>
        /// Clear the scheme's current symbol table. This should ALWAYS be done before serialing a new object hierarchy.
        /// </summary>
        public void ClearSymbolTable();
    }
}