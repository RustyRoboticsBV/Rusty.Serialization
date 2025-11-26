using System;
using System.Reflection;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// An object that can convert between objects of some type and serializer nodes.
/// </summary>
public interface IConverter
{
    /* Public properties. */
    /// <summary>
    /// The type that this converter can handle.
    /// </summary>
    public Type TargetType { get; }
    /// <summary>
    /// The type label that will be used for the targer type.
    /// </summary>
    public string TypeLabel
    {
        get
        {
            TypeName name = new(TargetType);
            var attribute = TargetType.GetCustomAttribute<SerializableAttribute>();
            if (attribute != null)
                name = name.Rename(attribute.Name);
            return name;
        }
    }

    /* Public methods. */
    /// <summary>
    /// Emit a serializer node representation for some object.
    /// </summary>
    public INode Convert(object obj, Context context);

    /// <summary>
    /// Emit a deserialized object from some serializer node.
    /// </summary>
    public object Deconvert(INode node, Context context);
}

/// <summary>
/// An object that can convert between objects of some type and serializer nodes.
/// </summary>
public interface IConverter<T> : IConverter
{
    /* Public properties. */
    Type IConverter.TargetType => typeof(T);

    /* Public methods. */
    INode IConverter.Convert(object obj, Context context) => Convert((T)obj, context);
    object IConverter.Deconvert(INode node, Context context) => Deconvert(node, context);

    /// <summary>
    /// Emit a serializer node representation for some object.
    /// </summary>
    public INode Convert(T obj, Context context);

    /// <summary>
    /// Emit a deserialized object from some serializer node.
    /// </summary>
    public new T Deconvert(INode node, Context context);
}