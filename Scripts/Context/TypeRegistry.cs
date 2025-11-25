#if GODOT && !UNITY_5_OR_NEWER
#define GODOT_CONTEXT

#elif !GODOT && UNITY_5_OR_NEWER
#define UNITY_CONTEXT
#endif

using System;
using System.Collections.Generic;
using System.Linq;

namespace Rusty.Serialization.Converters;

/// <summary>
/// An target type to IConverter type registry.
/// </summary>
public class TypeRegistry
{
    /* Private properties. */
    private Dictionary<Type, Type> targetToConverter = new();
    private Dictionary<Type, Type> cache = new();

    /* Constructors. */
    public TypeRegistry()
    {
        // Add built-in types.
        Add<bool, BoolConverter>();

        Add<sbyte, SbyteConverter>();
        Add<short, ShortConverter>();
        Add<int, IntConverter>();
        Add<long, LongConverter>();
        Add<byte, ByteConverter>();
        Add<ushort, UshortConverter>();
        Add<uint, UintConverter>();
        Add<ulong, UlongConverter>();

        Add<float, FloatConverter>();
        Add<double, DoubleConverter>();
        Add<decimal, DecimalConverter>();

        Add<char, CharConverter>();

        Add<string, StringConverter>();

        Add<DateTime, DateTimeConverter>();

        Add<byte[], ByteArrayConverter>();

        Add<System.Drawing.Color, ColorConverter>();
#if GODOT_CONTEXT
        Add<Godot.Color, Gd.ColorConverter>();
#endif

        Add(typeof(List<>), typeof(ListConverter<>));
        Add(typeof(LinkedList<>), typeof(LinkedListConverter<>));
        Add(typeof(HashSet<>), typeof(HashSetConverter<>));
        Add(typeof(Stack<>), typeof(StackConverter<>));
        Add(typeof(Queue<>), typeof(QueueConverter<>));
#if GODOT_CONTEXT
        Add(typeof(Godot.Collections.Array), typeof(Gd.ArrayConverter));
        Add(typeof(Godot.Collections.Array<>), typeof(Gd.ArrayConverter<>));
#endif

        Add(typeof(Dictionary<,>), typeof(DictionaryConverter<,>));
        Add(typeof(KeyValuePair<,>), typeof(KeyValuePairConverter<,>));
#if GODOT_CONTEXT
        Add(typeof(Godot.Collections.Dictionary), typeof(Gd.DictionaryConverter));
        Add(typeof(Godot.Collections.Dictionary<,>), typeof(Gd.DictionaryConverter<,>));
#endif
    }

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
        if (target.GetGenericArguments().Length != converter.GetGenericArguments().Length)
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
        Type converterType = Resolve(targetType);
        return Activator.CreateInstance(converterType) as IConverter;
    }

    /// <summary>
    /// Instantiate a registered converter and return it.
    /// </summary>
    public IConverter<TargetT> Instantiate<TargetT>()
    {
        Type type = typeof(TargetT);
        return Instantiate(type) as IConverter<TargetT>;
    }

    /* Private methods. */
    /// <summary>
    /// Try to resolve a target type and match it with a serializer.
    /// </summary>
    private Type Resolve(Type targetType)
    {
        // Cannot resolve open generic types.
        if (targetType.IsGenericTypeDefinition || targetType.ContainsGenericParameters)
            throw new Exception($"Cannot resolve open generic type '{targetType}'.");

        // Cache.
        if (cache.ContainsKey(targetType))
            return cache[targetType];

        // Direct resolve.
        if (targetToConverter.ContainsKey(targetType))
        {
            cache[targetType] = targetToConverter[targetType];
            return targetToConverter[targetType];
        }

        // Resolve enum types.
        if (targetType.IsEnum)
        {
            Type enumConverterType = typeof(EnumConverter<>).MakeGenericType(targetType);
            cache[targetType] = enumConverterType;
            return enumConverterType;
        }

        // Resolve array types.
        if (targetType.IsArray)
        {
            Type elementType = targetType.GetElementType();
            Type arrayConverterType = typeof(ArrayConverter<>).MakeGenericType(elementType);
            cache[targetType] = arrayConverterType;
            return arrayConverterType;
        }

        // Resolve tuple types.
        if (targetType.IsValueType && targetType.IsGenericType &&
            targetType.FullName!.StartsWith("System.ValueTuple`"))
        {
            Type valueTupleConverterType = typeof(TupleConverter<>).MakeGenericType(targetType);
            cache[targetType] = valueTupleConverterType;
            return valueTupleConverterType;
        }

        // Resolve closed generic types.
        if (targetType.IsGenericType)
        {
            Type openGenericType = targetType.GetGenericTypeDefinition();
            if (targetToConverter.ContainsKey(openGenericType))
            {
                Type genericConverterType = targetToConverter[openGenericType];
                Type[] typeArguments = targetType.GetGenericArguments();
                Type closedConverterType = genericConverterType.MakeGenericType(typeArguments);
                cache[targetType] = closedConverterType;
                return closedConverterType;
            }
        }

        // Resolve inherited types.
        Type parentType = targetType.BaseType;
        while (parentType != null)
        {
            if (targetToConverter.ContainsKey(parentType))
            {
                cache[targetType] = targetToConverter[parentType];
                return targetToConverter[parentType];
            }
            parentType = parentType.BaseType;
        }

        // Resolve unregistered struct type.
        if (targetType.IsValueType)
        {
            Type structConverterType = typeof(StructConverter<>).MakeGenericType(targetType);
            cache[targetType] = structConverterType;
            return cache[targetType];
        }

        // Resolve unregistered class type.
        if (targetType.IsClass)
        {
            Type classConverterType = typeof(ClassConverter<>).MakeGenericType(targetType);
            cache[targetType] = classConverterType;
            return cache[targetType];
        }

        // Could not resolve.
        throw new Exception($"Could not find a converterType for type '{targetType}'.");
    }
}