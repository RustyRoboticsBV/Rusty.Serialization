using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An object that can convert between objects of some type and serializer nodes.
    /// </summary>
    public interface IConverter
    {
        /* Public properties. */
        /// <summary>
        /// The type that this converter can handle.
        /// </summary>
        public Type TargetType { get; }

        /* Public methods. */
        /// <summary>
        /// Emit a serializer node representation for some object.
        /// </summary>
        public INode Convert(object obj, IConverterScheme scheme);

        /// <summary>
        /// Emit a deserialized object from some serializer node.
        /// </summary>
        public object Deconvert(INode node, IConverterScheme scheme);
    }

    /// <summary>
    /// An object that can convert between objects of some type and serializer nodes.
    /// </summary>
    public interface IConverter<T> : IConverter
    {
        /* Public methods. */
        /// <summary>
        /// Emit a serializer node representation for some object.
        /// </summary>
        public INode Convert(T obj, IConverterScheme scheme);

        /// <summary>
        /// Emit a deserialized object from some serializer node.
        /// </summary>
        public new T Deconvert(INode node, IConverterScheme scheme);
    }
}