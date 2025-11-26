using System;
using Rusty.Serialization.Nodes;
using Rusty.Serialization.Converters;

namespace Rusty.Serialization;

/// <summary>
/// A serialization context. It can be used to serialize and deserialize objects.
/// </summary>
public class Context
{
    /* Private properties. */
    /// <summary>
    /// The registry of known converter types.
    /// </summary>
    private TypeRegistry Types { get; set; }
    /// <summary>
    /// The registry of known converter instances.
    /// </summary>
    private InstanceRegistry Instances { get; } = new();

    /* Constructors. */
    public Context() : this(new()) { }

    public Context(TypeRegistry registry)
    {
        Types = registry;
    }

    /* Public methods. */
    public void AddConverter(Type targetT, Type converterT)
    {
        Types.Add(targetT, converterT);
    }

    public void AddConverter<TargetT, ConverterT>()
        where ConverterT : IConverter<TargetT>
    {
        Types.Add<TargetT, ConverterT>();
    }

    public IConverter GetConverter(Type targetType)
    {
        IConverter instance = Instances.Get(targetType);
        if (instance == null)
        {
            instance = Types.Instantiate(targetType);
            Instances.Add(targetType, instance);
        }
        return instance;
    }

    public TypeName GetTypeName(Type targetType)
    {
        return Types.GetTypeName(targetType);
    }

    /// <summary>
    /// Serialize an object.
    /// </summary>
    public string Serialize(object obj)
    {
        // Get converter.
        IConverter converter = GetConverter(obj.GetType());

        // Convert object to node.
        INode node = converter.Convert(obj, this);

        // Serialize node.
        return node.Serialize();
    }

    /// <summary>
    /// Serialize an object.
    /// </summary>
    public string Serialize<T>(T obj)
    {
        // Get converter.
        IConverter converter = GetConverter(obj.GetType());

        // Convert object to node.
        INode node = converter.Convert(obj, this);

        // Serialize node.
        return node.Serialize();
    }
}