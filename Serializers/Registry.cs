using System;
using System.Collections.Generic;
using System.Linq;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A registry of serializers for various types.
/// </summary>
public sealed class Registry
{
    /* Fields. */
    private Dictionary<Type, Type> serializerTypes = new();
    private Dictionary<Type, ISerializer> serializerInstances = new();
    private List<Type> order = new();

    /* Constructors. */
    public Registry(IEnumerable<(Type, Type)> serializers = null)
    {
        // Add built-in primitive serializers.
        AddSerializer<bool, BoolSerializer>();

        AddSerializer<sbyte, SbyteSerializer>();
        AddSerializer<byte, ByteSerializer>();
        AddSerializer<short, ShortSerializer>();
        AddSerializer<ushort, UshortSerializer>();
        AddSerializer<int, IntSerializer>();
        AddSerializer<uint, UintSerializer>();
        AddSerializer<long, LongSerializer>();
        AddSerializer<ulong, UlongSerializer>();

        AddSerializer<float, FloatSerializer>();
        AddSerializer<double, DoubleSerializer>();
        AddSerializer<decimal, DecimalSerializer>();

        AddSerializer<char, CharSerializer>();

        AddSerializer<string, StringSerializer>();

        // Add built-in collection serializers.
        AddSerializer(typeof(List<>), typeof(ListSerializer<>));

        AddSerializer(typeof(Dictionary<,>), typeof(DictionarySerializer<,>));

        // Add Godot serializers.
#if GODOT
        AddSerializer<Godot.StringName, Gd.StringNameSerializer>();
        AddSerializer<Godot.NodePath, Gd.NodePathSerializer>();

        AddSerializer<Godot.Vector2, Gd.Vector2Serializer>();
        AddSerializer<Godot.Vector3, Gd.Vector3Serializer>();
        AddSerializer<Godot.Vector4, Gd.Vector4Serializer>();
        AddSerializer<Godot.Vector2I, Gd.Vector2ISerializer>();
        AddSerializer<Godot.Vector3I, Gd.Vector3ISerializer>();
        AddSerializer<Godot.Vector4I, Gd.Vector4ISerializer>();

        AddSerializer<Godot.Quaternion, Gd.QuaternionSerializer>();
        AddSerializer<Godot.Plane, Gd.PlaneSerializer>();

        AddSerializer<Godot.Rect2, Gd.Rect2Serializer>();
        AddSerializer<Godot.Rect2I, Gd.Rect2ISerializer>();
        AddSerializer<Godot.Aabb, Gd.AabbSerializer>();

        AddSerializer<Godot.Transform2D, Gd.Transform2DSerializer>();
        AddSerializer<Godot.Basis, Gd.BasisSerializer>();
        AddSerializer<Godot.Transform3D, Gd.Transform3DSerializer>();
        AddSerializer<Godot.Projection, Gd.ProjectionSerializer>();

        AddSerializer<Godot.Color, Gd.ColorSerializer>();

        AddSerializer(typeof(Godot.Collections.Array<>), typeof(Gd.ArraySerializer<>));
        AddSerializer(typeof(Godot.Collections.Dictionary<,>), typeof(Gd.DictionarySerializer<,>));
#endif

        // Add Unity serializers.
#if UNITY_5_OR_NEWER
        AddSerializer<UnityEngine.Color, Unity.ColorSerializer>();
        AddSerializer<UnityEngine.Color32, Unity.Color32Serializer>();
#endif
    }

    /* Public methods. */
#if DEBUG
    public void PrintSerializers()
    {
        int index = 0;
        foreach (var pair in serializerInstances)
        {
            Console.WriteLine(index + ": " + pair.Key + "\n    " + pair.Value);
            index++;
        }
    }
#endif

    /// <summary>
    /// Add a serializer for some type.
    /// </summary>
    public void AddSerializer<TargetT, SerializerT>()
        where SerializerT : ISerializer
    {
        AddSerializer(typeof(TargetT), typeof(SerializerT));
    }

    /// <summary>
    /// Add a serializer for some type.
    /// </summary>
    public void AddSerializer(Type targetType, Type serializerType)
    {
        if (serializerType.GetInterface(nameof(ISerializer)) == null)
            throw new ArgumentException($"The type '{serializerType}' does not implement {nameof(ISerializer)}.");

        serializerTypes[targetType] = serializerType;
        order.Add(targetType);
    }

    /// <summary>
    /// Try to get a serializer for some type.
    /// </summary>
    public bool TryGetSerializer(Type type, out ISerializer serializer)
    {
        // Try to retrieve the serializer for this type.
        serializer = ResolveSerializer(type);

        // Return whether or not we succeeded.
        return serializer != null;
    }

    /// <summary>
    /// Get a serializer for some type.
    /// </summary>
    public ISerializer GetSerializer(Type type)
    {
        // Try to retrieve the serializer for this type.
        ISerializer serializer = ResolveSerializer(type);
        if (serializer != null)
            return serializer;

        // Else, throw exception.
        throw new ArgumentException($"No serializerType for arrayType '{type}' could be found.");

    }

    /* Private methods. */
    /// <summary>
    /// Find the best serializer for some type.
    /// </summary>
    private ISerializer ResolveSerializer(Type type)
    {
        // Fast path: check instances.
        if (serializerInstances.TryGetValue(type, out ISerializer instance))
            return instance;

        Type serializer = null;

        // Step 1: exact match?
        if (serializerTypes.TryGetValue(type, out serializer))
            return serializerInstances[type] = CreateInstance(type, serializer);

        // Step 2: generic type definition match.
        if (type.IsGenericType)
        {
            var genericDef = type.GetGenericTypeDefinition();
            if (serializerTypes.TryGetValue(genericDef, out serializer))
                return serializerInstances[type] = CreateInstance(type, serializer);
        }

        // Step 3: walk up the base class chain.
        Type baseType = type.BaseType;
        while (baseType != null)
        {
            if (serializerTypes.TryGetValue(baseType, out serializer))
                return serializerInstances[type] = CreateInstance(type, serializer);

            baseType = baseType.BaseType;
        }

        // Step 4: interface lookup in order of registration.
        Type[] implementedInterfaces = type.GetInterfaces();

        foreach (Type ordered in order)
        {
            if (ordered.IsInterface && implementedInterfaces.Contains(ordered))
                return serializerInstances[type] = CreateInstance(type, ordered);
        }

        // Case 5: array serialization.
        if (type.IsArray)
            return serializerInstances[type] = CreateArraySerializer(type);

        // Case 6: enum serialization.
        if (type.IsEnum)
            return serializerInstances[type] = CreateEnumSerializer(type);

        // Case 7: default class/struct serialization.
        if ((type.IsClass && !type.IsAbstract) || (type.IsValueType && !type.IsPrimitive && !type.IsEnum))
            return serializerInstances[type] = CreateObjectSerializer(type);

        // No serializer found.
        return serializerInstances[type] = null;
    }

    private ISerializer CreateInstance(Type targetType, Type serializerType)
    {
        // Case 1: serializer is NOT generic.
        if (!targetType.IsGenericTypeDefinition && !serializerType.IsGenericTypeDefinition)
            return (ISerializer)Activator.CreateInstance(serializerType);

        // Case 2: target type is a generic type.
        if (targetType.IsGenericType && serializerType.IsGenericType)
            return CreateGenericSerializer(targetType, serializerType);

        // This should never happen unless the registry is misconfigured.
        throw new InvalidOperationException(
            $"Could not create serializer '{serializerType}' for target type '{targetType}'."
        );
    }

    /// <summary>
    /// Create an array serializer for some array type.
    /// </summary>
    private ISerializer CreateArraySerializer(Type arrayType)
    {
        // Get element type.
        Type elementType = arrayType.GetElementType();

        // Create ArraySerializer<T> closed generic.
        Type closed = typeof(ArraySerializer<>).MakeGenericType(elementType);

        // Construct instance.
        return (ISerializer)Activator.CreateInstance(closed);
    }

    private ISerializer CreateGenericSerializer(Type genericTargetType, Type genericSerializerType)
    {
        Type[] args = genericTargetType.GetGenericArguments();
        Type closedSerializer = genericSerializerType.MakeGenericType(args);
        return (ISerializer)Activator.CreateInstance(closedSerializer);
    }

    /// <summary>
    /// Create an enum serializer for some enum type.
    /// </summary>
    private ISerializer CreateEnumSerializer(Type enumType)
    {
        // Create ObjectSerializer<T> closed generic.
        Type serializerType = typeof(EnumSerializer<>).MakeGenericType(enumType);

        // Construct instance.
        return (ISerializer)Activator.CreateInstance(serializerType);
    }

    /// <summary>
    /// Create an object serializer for some object type.
    /// </summary>
    private ISerializer CreateObjectSerializer(Type objectType)
    {
        // Create ObjectSerializer<T> closed generic.
        Type serializerType = typeof(ObjectSerializer<>).MakeGenericType(objectType);

        // Construct instance.
        return (ISerializer)Activator.CreateInstance(serializerType);
    }
}