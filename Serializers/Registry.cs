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
        AddSerializer(typeof(Array), typeof(ArraySerializer<>));
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
        throw new ArgumentException($"No serializerType for targetType '{type}' could be found.");

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

        // Step 2: walk up the base class chain.
        Type baseType = type.BaseType;
        while (baseType != null)
        {
            if (serializerTypes.TryGetValue(baseType, out serializer))
                return serializerInstances[type] = CreateInstance(type, serializer);

            baseType = baseType.BaseType;
        }

        // Step 3: interface lookup in order of registration.
        Type[] implementedInterfaces = type.GetInterfaces();

        foreach (Type ordered in order)
        {
            if (ordered.IsInterface && implementedInterfaces.Contains(ordered))
                return serializerInstances[type] = CreateInstance(type, ordered);
        }

        // Step 4: generic type definition match.
        if (type.IsGenericType)
        {
            var genericDef = type.GetGenericTypeDefinition();
            if (serializerTypes.TryGetValue(genericDef, out serializer))
                return serializerInstances[type] = CreateInstance(type, serializer);
        }

        // Case 5: enum serialization.
        if (type.IsEnum)
            return serializerInstances[type] = CreateAutoEnumSerializer(type);

        // Case 6: default class/struct serialization.
        if ((type.IsClass && !type.IsAbstract) || (type.IsValueType && !type.IsPrimitive && !type.IsEnum))
            return serializerInstances[type] = CreateAutoObjectSerializer(type);


        // No serializer found.
        return serializerInstances[type] = null;
    }

    private ISerializer CreateInstance(Type targetType, Type serializerType)
    {
        // Case 1: serializer is NOT generic.
        if (!serializerType.IsGenericTypeDefinition)
            return (ISerializer)Activator.CreateInstance(serializerType);

        // Case 2: type is an array.
        if (targetType.IsArray)
        {
            Type elem = targetType.GetElementType();
            Type closed = serializerType.MakeGenericType(elem);
            return (ISerializer)Activator.CreateInstance(closed);
        }

        // Case 3: target type is a generic type.
        if (targetType.IsGenericType)
        {
            Type[] args = targetType.GetGenericArguments();
            Type closed = serializerType.MakeGenericType(args);
            return (ISerializer)Activator.CreateInstance(closed);
        }

        // This should never happen unless the registry is misconfigured.
        throw new InvalidOperationException(
            $"Serializer targetType '{serializerType}' is generic, but target targetType '{targetType}' cannot supply targetType arguments."
        );
    }

    private ISerializer CreateAutoEnumSerializer(Type targetType)
    {
        // Create ObjectSerializer<T> closed generic.
        Type serializerType = typeof(EnumSerializer<>).MakeGenericType(targetType);

        // Construct instance.
        ISerializer instance = (ISerializer)Activator.CreateInstance(serializerType, (TypeName)targetType.FullName);

        return instance;
    }

    private ISerializer CreateAutoObjectSerializer(Type targetType)
    {
        // Create ObjectSerializer<T> closed generic.
        Type serializerType = typeof(ObjectSerializer<>).MakeGenericType(targetType);

        // Construct instance.
        ISerializer instance = (ISerializer)Activator.CreateInstance(serializerType, [null, null]);

        return instance;
    }
}