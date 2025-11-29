using System;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A base class for all converters.
    /// </summary>
    public abstract class Converter<T> : IConverter<T>
    {
        /* Public properties. */
        public Type TargetType => typeof(T);

        /* Public methods */
        INode IConverter.Convert(object obj, IConverterScheme scheme) => Convert((T)obj, scheme);
        object IConverter.Deconvert(INode node, IConverterScheme scheme) => Deconvert(node, scheme);

        public abstract INode Convert(T obj, IConverterScheme scheme);

        public abstract T Deconvert(INode node, IConverterScheme scheme);

        /* Protected methods. */
        /// <summary>
        /// Convert an object into a node.
        /// </summary>
        protected INode ConvertNested<U>(Type expectedType, U obj, IConverterScheme scheme)
        {
            // Convert obj to node.
            INode node = scheme.Convert(obj);

            // Wrap inside of a type node if there was a mismatch.
            Type valueType = obj?.GetType();
            if (expectedType != valueType && valueType != null)
                node = new TypeNode(scheme.GetTypeName(valueType), node);

            // Return finished node.
            return node;
        }

        /// <summary>
        /// Deconvert an object into an element.
        /// </summary>
        protected U DeconvertNested<U>(INode node, IConverterScheme scheme)
        {
            if (node is TypeNode typed)
                return (U)DeconvertNested(scheme.GetTypeFromName(typed.Name), typed.Value, scheme);
            return scheme.Deconvert<U>(node);
        }

        /// <summary>
        /// Deconvert an object into an element.
        /// </summary>
        protected object DeconvertNested(Type type, INode node, IConverterScheme scheme)
        {
            if (node is TypeNode typed)
            {
                Type nestedType = scheme.GetTypeFromName(typed.Name);
                System.Console.WriteLine(type + " : " + typed.Name + " => " + (nestedType != null ? nestedType : "null") + "\n" + node);
                return DeconvertNested(scheme.GetTypeFromName(typed.Name), typed.Value, scheme);
            }
            return scheme.Deconvert(type, node);
        }
    }
}