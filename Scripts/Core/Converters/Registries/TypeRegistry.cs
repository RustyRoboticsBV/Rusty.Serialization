using System;
using System.Collections.Generic;
using System.Linq;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// An target type to IConverter type registry.
    /// </summary>
    public class TypeRegistry
    {
        /* Private properties. */
        private Dictionary<Type, Type> targetToConverter = new();

        /* Public methods. */
        /// <summary>
        /// Register a converter type for some target type.
        /// </summary>
        public void Add<TargetT, ConverterT>()
            where ConverterT : IConverter
        {
            Add(typeof(TargetT), typeof(ConverterT));
        }

        /// <summary>
        /// Register a converter type for some target type.
        /// </summary>
        public void Add(Type target, Type converter)
        {
            // Only allow converter types that implement IConverter.
            if (!converter.GetInterfaces().Any(i => i == typeof(IConverter)))
                throw new Exception($"Type '{converter}' does not implement interface {nameof(IConverter)}!");

            // Only allow target and converter types with the same number of generic arguments.
            // Exception: allow (T, Converter<T>) pairs.
            Type[] targetGenericArgs = target.GetGenericArguments();
            Type[] converterGenericArgs = converter.GetGenericArguments();
            if (!(targetGenericArgs.Length == 0 && converterGenericArgs.Length == 1)
                && targetGenericArgs.Length != converterGenericArgs.Length)
            {
                throw new Exception($"Target type '{target}' and converterType type '{converter}' did not have the same number of "
                    + "generic type arguments.");
            }

            // Add the converter type.
            targetToConverter[target] = converter;
        }

        /// <summary>
        /// Instantiate a registered converter and return it.
        /// </summary>
        public IConverter Instantiate(Type targetType)
        {
            Type converterType = ResolveConverter(targetType);

            // Close generic converters with 1 generic argument using the target type.
            if (converterType.IsGenericTypeDefinition && converterType.GetGenericArguments().Length == 1)
                converterType = converterType.MakeGenericType(targetType);

            // Construct a converter.
            return Activator.CreateInstance(converterType, true) as IConverter;
        }

        /// <summary>
        /// Instantiate a registered converter and return it.
        /// </summary>
        public IConverter Instantiate<TargetT>() => Instantiate(typeof(TargetT));

        /* Private methods. */
        /// <summary>
        /// Try to resolve a target type and match it with a serializer.
        /// </summary>
        private Type ResolveConverter(Type targetType)
        {
            // Resolve null.
            if (targetType == null)
                return null;

            // Cannot resolve open generic types.
            if (targetType.IsGenericTypeDefinition || targetType.ContainsGenericParameters)
                throw new Exception($"Cannot resolve open generic type '{targetType}'.");

            // Direct resolve.
            if (targetToConverter.ContainsKey(targetType))
                return targetToConverter[targetType];

            // Resolve nullable types.
            if (Nullable.GetUnderlyingType(targetType) is Type underlyingType)
                return typeof(NullableConverter<>).MakeGenericType(underlyingType);

            // Resolve enum types.
            if (targetType.IsEnum)
                return typeof(EnumConverter<>).MakeGenericType(targetType);

            // Resolve array types.
            if (targetType.IsArray)
            {
                Type elementType = targetType.GetElementType();
                return typeof(ArrayConverter<>).MakeGenericType(elementType);
            }

            // Resolve tuple types.
            if (targetType.IsValueType && targetType.IsGenericType &&
                targetType.FullName!.StartsWith("System.ValueTuple`"))
            {
                return typeof(ValueTupleConverter<>).MakeGenericType(targetType);
            }
            if (targetType.IsClass && targetType.IsGenericType &&
                targetType.FullName!.StartsWith("System.Tuple`"))
            {
                return typeof(ReferenceTupleConverter<>).MakeGenericType(targetType);
            }

            // Resolve closed generic types.
            if (targetType.IsGenericType)
            {
                Type openGenericType = targetType.GetGenericTypeDefinition();
                if (targetToConverter.ContainsKey(openGenericType))
                {
                    Type genericConverterType = targetToConverter[openGenericType];
                    Type[] typeArguments = targetType.GetGenericArguments();
                    return genericConverterType.MakeGenericType(typeArguments);
                }
            }

            // Resolve inherited types.
            Type parentType = targetType.BaseType;
            while (parentType != null)
            {
                if (targetToConverter.ContainsKey(parentType))
                    return targetToConverter[parentType];
                parentType = parentType.BaseType;
            }

            // Resolve unregistered struct type.
            if (targetType.IsValueType)
                return typeof(StructConverter<>).MakeGenericType(targetType);

            // Resolve unregistered class type.
            if (targetType.IsClass)
                return typeof(ClassConverter<>).MakeGenericType(targetType);

            // Could not resolve.
            throw new Exception($"Could not find a converterType for type '{targetType}'.");
        }
    }
}