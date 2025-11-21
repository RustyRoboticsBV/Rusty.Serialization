using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A registry of serializers for various types.
/// </summary>
public sealed class Registry
{
    /* Fields. */
    private Dictionary<Type, Type> serializerTypes = new();
    private Dictionary<Type, ISerializer> serializerInstances = new();
    private Dictionary<Type, string> typeToName = new();
    private Dictionary<string, Type> nameToType = new();
    private List<Type> order = new();

    /* Constructors. */
    public Registry(IEnumerable<(Type, Type)> serializers = null)
    {
        // Add built-in primitive serializers.
        AddSerializer<bool, BoolSerializer>("b");

        AddSerializer<sbyte, SbyteSerializer>("i8");
        AddSerializer<byte, ByteSerializer>("i16");
        AddSerializer<short, ShortSerializer>("i32");
        AddSerializer<ushort, UshortSerializer>("i64");
        AddSerializer<int, IntSerializer>("u8");
        AddSerializer<uint, UintSerializer>("u16");
        AddSerializer<long, LongSerializer>("u32");
        AddSerializer<ulong, UlongSerializer>("u64");

        AddSerializer<float, FloatSerializer>("f16");
        AddSerializer<double, DoubleSerializer>("f64");
        AddSerializer<decimal, DecimalSerializer>("dec");

        AddSerializer<char, CharSerializer>("chr");

        AddSerializer<string, StringSerializer>("str");

        // Add built-in collection serializers.
        AddSerializer(typeof(List<>), typeof(ListSerializer<>));

        AddSerializer(typeof(Dictionary<,>), typeof(DictionarySerializer<,>));

        // Add Godot serializers.
#if GODOT
        AddSerializer<Godot.StringName, Gd.StringNameSerializer>();
        AddSerializer<Godot.NodePath, Gd.NodePathSerializer>();

        AddSerializer<Godot.Vector2, Gd.Vector2Serializer>("vec2f");
        AddSerializer<Godot.Vector3, Gd.Vector3Serializer>("vec3f");
        AddSerializer<Godot.Vector4, Gd.Vector4Serializer>("vec4f");
        AddSerializer<Godot.Vector2I, Gd.Vector2ISerializer>("vec2i");
        AddSerializer<Godot.Vector3I, Gd.Vector3ISerializer>("vec3i");
        AddSerializer<Godot.Vector4I, Gd.Vector4ISerializer>("vec4i");

        AddSerializer<Godot.Quaternion, Gd.QuaternionSerializer>("quat");
        AddSerializer<Godot.Plane, Gd.PlaneSerializer>("pln");

        AddSerializer<Godot.Rect2, Gd.Rect2Serializer>("rec2f");
        AddSerializer<Godot.Rect2I, Gd.Rect2ISerializer>("rec2i");
        AddSerializer<Godot.Aabb, Gd.AabbSerializer>("aabb");

        AddSerializer<Godot.Transform2D, Gd.Transform2DSerializer>("mat23");
        AddSerializer<Godot.Basis, Gd.BasisSerializer>("mat33");
        AddSerializer<Godot.Transform3D, Gd.Transform3DSerializer>("mat34");
        AddSerializer<Godot.Projection, Gd.ProjectionSerializer>("mat44");

        AddSerializer<Godot.Color, Gd.ColorSerializer>("col");

        AddSerializer(typeof(Godot.Collections.Array<>), typeof(Gd.ArraySerializer<>));
        AddSerializer(typeof(Godot.Collections.Dictionary<,>), typeof(Gd.DictionarySerializer<,>));
#endif

        // Add Unity serializers.
#if UNITY_5_OR_NEWER
        AddSerializer<UnityEngine.Color, Unity.ColorSerializer>("col");
        AddSerializer<UnityEngine.Color32, Unity.Color32Serializer>("col32");
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
    public void AddSerializer<TargetT, SerializerT>(string typeName = null)
        where SerializerT : ISerializer
    {
        AddSerializer(typeof(TargetT), typeof(SerializerT), typeName);
    }

    /// <summary>
    /// Add a serializer for some type.
    /// </summary>
    public void AddSerializer(Type targetType, Type serializerType, string typeName = null)
    {
        if (typeName == null)
        {
            var serializableAttribute = targetType.GetCustomAttribute<SerializableAttribute>();
            if (serializableAttribute != null)
                typeName = serializableAttribute.TypeCode;
            else
                typeName = new TypeName(targetType);
        }

        if (serializerType.GetInterface(nameof(ISerializer)) == null)
            throw new ArgumentException($"The typeCode '{serializerType}' does not implement {nameof(ISerializer)}.");

        serializerTypes[targetType] = serializerType;
        typeToName[targetType] = typeName;
        nameToType[typeName] = targetType;
        order.Add(targetType);
    }

    /// <summary>
    /// Get a type from some type code.
    /// </summary>
    public Type FindType(string typeCode)
    {
        try
        {
            return nameToType[typeCode];
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Get the type code of some type.
    /// </summary>
    public TypeName GetTypeCode(Type type)
    {
        try
        {
            return typeToName[type];
        }
        catch
        {
            return new(type);
        }
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
        throw new ArgumentException($"No serializer for typeCode '{type}' could be found.");
    }

    /// <summary>
    /// Get a serializer for some type code.
    /// </summary>
    public ISerializer GetSerializer(string typeCode)
    {
        return GetSerializer(FindType(typeCode));
    }

    /// <summary>
    /// Try to get a serializer for some type.
    /// </summary>
    public bool TryGetSerializer(Type type, out ISerializer serializer)
    {
        try
        {
            serializer = GetSerializer(type);
            return serializer != null;
        }
        catch
        {
            serializer = null;
            return false;
        }
    }

    /// <summary>
    /// Try to get a serializer for some type.
    /// </summary>
    public bool TryGetSerializer(string typeCode, out ISerializer serializer)
    {
        return TryGetSerializer(FindType(typeCode), out serializer);
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
        {
            AddSerializer(type, typeof(ObjectSerializer<>).MakeGenericType(type));
            return serializerInstances[type] = CreateObjectSerializer(type);
        }

        // No serializer found.
        return serializerInstances[type] = null;
    }

    /// <summary>
    /// Create a serializer instance.
    /// </summary>
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
            $"Could not create serializer '{serializerType}' for target typeCode '{targetType}'."
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

    /// <summary>
    /// Create a serializer for some generic type.
    /// </summary>
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