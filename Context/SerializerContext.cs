using Rusty.Serialization.Nodes;
using Rusty.Serialization.Serializers;

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
        ISerializer serializer = Registry.GetSerializer(obj.GetType());
        INode node = serializer.Serialize(obj, Registry);
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
}