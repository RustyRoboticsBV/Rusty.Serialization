using Rusty.Serialization.Nodes;
using Rusty.Serialization.Serializers;
using System;
using System.Text;

namespace Rusty.Serialization;

/// <summary>
/// A serialization context. It can be used to serialize and deserialize objects.
/// </summary>
public class SerializerContext
{
    /* Public properties. */
    /// <summary>
    /// The registry of known serializers.
    /// </summary>
    public Registry Registry { get; private set; }

    /* Constructors. */
    public SerializerContext() : this(new()) { }

    public SerializerContext(Registry registry)
    {
        Registry = registry;
    }

    /* Public methods. */
    /// <summary>
    /// Register a serializer for some type.
    /// </summary>
    public void RegisterSerializer<TargetT, SerializerT>()
        where SerializerT : ISerializer
    {
        Registry.AddSerializer<TargetT, SerializerT>();
    }

    /// <summary>
    /// Serialize an object.
    /// </summary>
    public string Serialize(object obj)
    {
        Type type = obj?.GetType();

        // Convert to node.
        ISerializer serializer = Registry.GetSerializer(type);
        INode node = serializer.Serialize(obj, Registry);

        // Add top-level type node.
        if (node is not NullNode)
            node = new TypeNode(Registry.GetTypeCode(type), node);

        // Serialize node.
        return node.Serialize();
    }

    /// <summary>
    /// Deserialize an object.
    /// </summary>
    public T Deserialize<T>(string serialized)
    {
        INode node = ParseUtility.ParseValue(serialized);
        ISerializer serializer = Registry.GetSerializer(typeof(T));
        return (T)serializer.Deserialize(node, Registry);
    }

    /// <summary>
    /// Encode string into ISO-8859-1 format.
    /// </summary>
    public static byte[] Encode(string str)
    {
        Encoding iso = Encoding.GetEncoding("ISO-8859-1");
        Encoding utf8 = Encoding.UTF8;
        byte[] utfBytes = utf8.GetBytes(str);
        return Encoding.Convert(utf8, iso, utfBytes);
    }
}