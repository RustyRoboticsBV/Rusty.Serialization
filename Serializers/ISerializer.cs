using System;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// An object serializer.
/// </summary>
public interface ISerializer
{
    /* Public properties. */
    /// <summary>
    /// The type that this serializer can handle.
    /// </summary>
    public Type TargetType { get; }

    /* Public methods. */
    /// <summary>
    /// Emit a serializer node for an object.
    /// </summary>
    public INode Serialize(object obj, Registry context);

    /// <summary>
    /// Emit a deserialized object.
    /// </summary>
    public object Deserialize(INode node, Registry context);
}

/// <summary>
/// An object serializer.
/// </summary>
public interface ISerializer<T> : ISerializer
{
    /* Public properties. */
    Type ISerializer.TargetType => typeof(T);

    /* Public methods. */
    INode ISerializer.Serialize(object obj, Registry context) => Serialize((T)obj, context);
    object ISerializer.Deserialize(INode node, Registry context) => Deserialize(node, context);

    /// <summary>
    /// Emit a serializer node for an object.
    /// </summary>
    public INode Serialize(T obj, Registry context);

    /// <summary>
    /// Emit a deserialized object.
    /// </summary>
    public new T Deserialize(INode node, Registry context);
}