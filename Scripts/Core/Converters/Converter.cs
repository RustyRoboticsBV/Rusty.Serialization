using Rusty.Serialization.Core.Nodes;
using System;
using System.Reflection;
using System.Linq;

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
            object obj;

            // Unwrap type node.
            if (node is TypeNode typed)
            {
                Type underlyingType = scheme.GetTypeFromName(typed.Name);
                obj = DeconvertNested(underlyingType, typed.Value, scheme);
            }

            // Else, deconvert as-is.
            else
                obj = scheme.Deconvert<U>(node);

            // If the object is of the correct type, return it.
            if (obj is U u)
                return u;

            // Else, check if a conversion operator is available.
            object converted = DoConversionOperator(typeof(U), obj);
            if (converted is U typedConverted)
                return typedConverted;

            throw new InvalidCastException(
                $"Cannot convert node value of type {obj?.GetType().Name} to {typeof(U).Name}.");
        }

        /// <summary>
        /// Deconvert an object into an element.
        /// </summary>
        protected object DeconvertNested(Type type, INode node, IConverterScheme scheme)
        {
            if (node is TypeNode typed)
            {
                Type nestedType = scheme.GetTypeFromName(typed.Name);
                return DeconvertNested(scheme.GetTypeFromName(typed.Name), typed.Value, scheme);
            }
            return scheme.Deconvert(type, node);
        }

        /* Private methods. */
        /// <summary>
        /// Try to run an explicit or implicit conversion operator from some value to the target type.
        /// </summary>
        private static object DoConversionOperator(Type targetType, object value)
        {
            if (value == null)
                return null;

            Type sourceType = value.GetType();

            // Look for an implicit operator on the target type
            var method = targetType.GetMethods(
                BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(m =>
                    (m.Name == "op_Implicit" || m.Name == "op_Explicit") &&
                    m.ReturnType == targetType &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType.IsAssignableFrom(sourceType)
                );

            if (method != null)
                return method.Invoke(null, new[] { value });

            // Look for an implicit operator on the source type
            method = sourceType.GetMethods(
                BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault(m =>
                    (m.Name == "op_Implicit" || m.Name == "op_Explicit") &&
                    m.ReturnType == targetType &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == sourceType
                );

            if (method != null)
                return method.Invoke(null, new[] { value });

            // No conversion found
            throw new InvalidCastException(
                $"No implicit conversion operator found from {sourceType} to {targetType}.");
        }
    }
}