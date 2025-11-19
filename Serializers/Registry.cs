using System;
using System.Collections.Generic;
using System.Linq;

namespace Rusty.Serialization;

/// <summary>
/// A registry of serializers for various types.
/// </summary>
public sealed class Registry
{
    /* Fields. */
    private readonly Dictionary<Type, Type> serializerTypes = new();
    private readonly Dictionary<Type, ISerializer> serializerInstances = new();
    private readonly List<Type> order;

    /* Constructors. */
    public Registry(IEnumerable<Type> serializers = null)
    {
        order = serializers?.ToList() ?? new();

        // Add built-in serializers.
        serializerTypes[typeof(bool)] = typeof(BoolSerializer);

        serializerTypes[typeof(sbyte)] = typeof(SbyteSerializer);
        serializerTypes[typeof(byte)] = typeof(ByteSerializer);
        serializerTypes[typeof(short)] = typeof(ShortSerializer);
        serializerTypes[typeof(ushort)] = typeof(UshortSerializer);
        serializerTypes[typeof(int)] = typeof(IntSerializer);
        serializerTypes[typeof(uint)] = typeof(UintSerializer);
        serializerTypes[typeof(long)] = typeof(LongSerializer);
        serializerTypes[typeof(ulong)] = typeof(UlongSerializer);

        serializerTypes[typeof(float)] = typeof(FloatSerializer);
        serializerTypes[typeof(double)] = typeof(DoubleSerializer);
        serializerTypes[typeof(decimal)] = typeof(DecimalSerializer);

        serializerTypes[typeof(char)] = typeof(CharSerializer);

        serializerTypes[typeof(string)] = typeof(StringSerializer);

        serializerTypes[typeof(Array)] = typeof(ArraySerializer<>);
        serializerTypes[typeof(List<>)] = typeof(ListSerializer<>);

        serializerTypes[typeof(Dictionary<,>)] = typeof(DictionarySerializer<,>);

#if GODOT
        serializerTypes[typeof(Godot.Vector2)] = typeof(GodotEngine.Vector2Serializer);
        serializerTypes[typeof(Godot.Vector3)] = typeof(GodotEngine.Vector3Serializer);
        serializerTypes[typeof(Godot.Vector4)] = typeof(GodotEngine.Vector4Serializer);
        serializerTypes[typeof(Godot.Vector2I)] = typeof(GodotEngine.Vector2ISerializer);
        serializerTypes[typeof(Godot.Vector3I)] = typeof(GodotEngine.Vector3ISerializer);
        serializerTypes[typeof(Godot.Vector4I)] = typeof(GodotEngine.Vector4ISerializer);
        serializerTypes[typeof(Godot.Quaternion)] = typeof(GodotEngine.QuaternionSerializer);
        serializerTypes[typeof(Godot.Color)] = typeof(GodotEngine.ColorSerializer);
        serializerTypes[typeof(Godot.Collections.Array<>)] = typeof(GodotEngine.ArraySerializer<>);
        serializerTypes[typeof(Godot.Collections.Dictionary<,>)] = typeof(GodotEngine.DictionarySerializer<,>);
#endif
    }

    /* Public methods. */
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
        throw new ArgumentException($"No serializer for targetType '{type}' could be found.");

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
}