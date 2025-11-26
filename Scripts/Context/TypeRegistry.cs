#if GODOT && !UNITY_5_OR_NEWER
#define GODOT_CONTEXT

#elif !GODOT && UNITY_5_OR_NEWER
#define UNITY_CONTEXT
#endif

using Rusty.Serialization.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rusty.Serialization.Converters;

/// <summary>
/// An target type to IConverter type registry.
/// </summary>
public class TypeRegistry
{
    /* Private properties. */
    private Dictionary<Type, Type> targetToConverter = new();
    private BiDictionary<Type, TypeName> typeNames = new();

    /* Constructors. */
    public TypeRegistry()
    {
        // Add built-in types.
        Add<bool, BoolConverter>("b");

        Add<sbyte, SbyteConverter>("i8");
        Add<short, ShortConverter>("i16");
        Add<int, IntConverter>("i32");
        Add<long, LongConverter>("i64");
        Add<byte, ByteConverter>("u8");
        Add<ushort, UshortConverter>("u16");
        Add<uint, UintConverter>("u32");
        Add<ulong, UlongConverter>("u64");

        Add<float, FloatConverter>("f32");
        Add<double, DoubleConverter>("f64");
        Add<decimal, DecimalConverter>("dec");

        Add<char, CharConverter>("chr");

        Add<string, StringConverter>("str");

        Add<DateTime, DateTimeConverter>("dt");

        Add<byte[], ByteArrayConverter>("u8[]");

        Add<System.Drawing.Color, ColorConverter>("col");
#if GODOT_CONTEXT
        Add<Godot.Color, Gd.ColorConverter>("gdcol");
#endif

        Add(typeof(List<>), typeof(ListConverter<>), "list");
        Add(typeof(LinkedList<>), typeof(LinkedListConverter<>), "link");
        Add(typeof(HashSet<>), typeof(HashSetConverter<>), "hset");
        Add(typeof(Stack<>), typeof(StackConverter<>), "stack");
        Add(typeof(Queue<>), typeof(QueueConverter<>), "queue");
#if GODOT_CONTEXT
        Add(typeof(Godot.Collections.Array), typeof(Gd.ArrayConverter), "gdarr");
        Add(typeof(Godot.Collections.Array<>), typeof(Gd.ArrayConverter<>), "gdarr");
#endif

        Add(typeof(Dictionary<,>), typeof(DictionaryConverter<,>), "dict");
        Add(typeof(KeyValuePair<,>), typeof(KeyValuePairConverter<,>), "dict");
#if GODOT_CONTEXT
        Add(typeof(Godot.Collections.Dictionary), typeof(Gd.DictionaryConverter), "gddict");
        Add(typeof(Godot.Collections.Dictionary<,>), typeof(Gd.DictionaryConverter<,>), "gddict");
#endif
    }

    /* Public methods. */
    /// <summary>
    /// Register a converter type for some target type.
    /// </summary>
    public void Add<TargetT, ConverterT>(string typeName = null)
        where ConverterT : IConverter
    {
        Add(typeof(TargetT), typeof(ConverterT), typeName);
    }

    /// <summary>
    /// Register a converter type for some target type.
    /// </summary>
    public void Add(Type target, Type converter, string typeName = null)
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

        // Store type name.
        if (typeName == null)
            typeName = ResolveName(target);
        typeNames[target] = typeName;
    }

    /// <summary>
    /// Get the name of some type.
    /// </summary>
    public TypeName GetTypeName(Type type)
    {
        try
        {
            return typeNames[type];
        }
        catch
        {

            ResolveName(type);
            return typeNames[type];
        }
    }

    /// <summary>
    /// Instantiate a registered converter and return it.
    /// </summary>
    public IConverter Instantiate(Type targetType)
    {
        Type converterType = ResolveConverter(targetType);
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
    private Type ResolveConverter(Type targetType)
    {
        // Cannot resolve open generic types.
        if (targetType.IsGenericTypeDefinition || targetType.ContainsGenericParameters)
            throw new Exception($"Cannot resolve open generic type '{targetType}'.");

        // Direct resolve.
        if (targetToConverter.ContainsKey(targetType))
            return targetToConverter[targetType];

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
            return typeof(TupleConverter<>).MakeGenericType(targetType);
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

    /// <summary>
    /// Try to resolve a target type's name.
    /// </summary>
    private TypeName ResolveName(Type targetType)
    {
        // If the attribute is present, use that for the name.
        var attribute = targetType.GetCustomAttribute<SerializableAttribute>();
        if (attribute != null)
            return attribute.Name;

        // If nested, add the parent class name as a prefix.
        return "";
    }
}