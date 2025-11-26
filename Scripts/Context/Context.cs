#if GODOT && !UNITY_5_OR_NEWER
#define GODOT_CONTEXT

#elif !GODOT && UNITY_5_OR_NEWER
#define UNITY_CONTEXT
#endif

using Rusty.Serialization.Converters;
using Rusty.Serialization.Nodes;
using System;
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A serialization context. It can be used to serialize and deserialize objects.
/// </summary>
public class Context
{
    /* Internal properties. */
    /// <summary>
    /// The registry of known converter types.
    /// </summary>
    internal TypeRegistry Types { get; } = new();
    /// <summary>
    /// The registry of known converter instances.
    /// </summary>
    internal InstanceRegistry Instances { get; } = new();
    /// <summary>
    /// The registry of known type aliasses.
    /// </summary>
    internal AliasRegistry Aliasses { get; } = new();

    /* Constructors. */
    public Context()
    {
        AddBuiltInTypes();
    }

    /* Public methods. */
    public void Add(Type targetT, Type converterT, string alias = null)
    {
        Types.Add(targetT, converterT);
        if (alias != null)
            Aliasses.Add(targetT, alias);
    }

    public void Add<TargetT, ConverterT>(string alias = null)
        where ConverterT : IConverter<TargetT>
    {
        Add(typeof(TargetT), typeof(ConverterT), alias);
    }

    public IConverter GetConverter(string targetNameOrAlias)
    {
        Type type;
        if (Aliasses.Has(targetNameOrAlias))
            type = Aliasses.Get(targetNameOrAlias);
        else
        {
            TypeName targetName = new(targetNameOrAlias, Aliasses);
            type = targetName.GetType();
        }
        return GetConverter(type);
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

    /// <summary>
    /// Serialize an object.
    /// </summary>
    public string Serialize(object obj, Type expectedType = null)
    {
        Type objType = obj?.GetType();

        // Get converter.
        IConverter converter = GetConverter(objType);

        // Convert object to node.
        INode node = converter.Convert(obj, this);

        // Wrap in type node if necessary.
        if (objType != expectedType)
            node = new TypeNode(GetTypeName(objType), node);

        // Serialize node.
        return node.Serialize();
    }

    /// <summary>
    /// Serialize an object.
    /// </summary>
    public string Serialize<T>(T obj, Type expectedType = null)
    {
        Type objType = obj?.GetType();

        // Get converter.
        IConverter converter = GetConverter(objType);

        // Convert object to node.
        INode node = converter.Convert(obj, this);

        // Wrap in type node if necessary.
        if (objType != expectedType)
            node = new TypeNode(GetTypeName(objType), node);

        // Serialize node.
        return node.Serialize();
    }

    /// <summary>
    /// Get a type's name.
    /// </summary>
    public TypeName GetTypeName(Type type) => new(type, Aliasses);

    /// <summary>
    /// Get a type from a type name.
    /// </summary>
    public Type GetTypeFromName(string name) => (Type)new TypeName(name, Aliasses);

    /* Private methods. */
    private void AddBuiltInTypes()
    {
        // Add built-in types.
        Aliasses.Add(typeof(Object), "obj");
        Aliasses.Add(typeof(Enum), "enum");

        Add<bool, BoolConverter>("bl");

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
        Add<Godot.Color, Converters.Gd.ColorConverter>("GDcol");
#endif

        Add(typeof(List<>), typeof(ListConverter<>), "list");
        Add(typeof(LinkedList<>), typeof(LinkedListConverter<>), "lnls");
        Add(typeof(HashSet<>), typeof(HashSetConverter<>), "hset");
        Add(typeof(Stack<>), typeof(StackConverter<>), "stck");
        Add(typeof(Queue<>), typeof(QueueConverter<>), "queu");
#if GODOT_CONTEXT
        Add(typeof(Godot.Collections.Array), typeof(Converters.Gd.ArrayConverter), "GDarr");
        Add(typeof(Godot.Collections.Array<>), typeof(Converters.Gd.ArrayConverter<>), "GDarrT");
#endif

        Add(typeof(Dictionary<,>), typeof(DictionaryConverter<,>), "dict");
        Add(typeof(KeyValuePair<,>), typeof(KeyValuePairConverter<,>), "kvp");
#if GODOT_CONTEXT
        Add(typeof(Godot.Collections.Dictionary), typeof(Converters.Gd.DictionaryConverter), "GDdict");
        Add(typeof(Godot.Collections.Dictionary<,>), typeof(Converters.Gd.DictionaryConverter<,>), "GDdictT");
#endif
    }
}