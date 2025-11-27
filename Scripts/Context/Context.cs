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

        // Bool types.
        Add<bool, BoolConverter>("bl");

        // Int types.
        Add<sbyte, SbyteConverter>("i8");
        Add<short, ShortConverter>("i16");
        Add<int, IntConverter>("i32");
        Add<long, LongConverter>("i64");
        Add<byte, ByteConverter>("u8");
        Add<ushort, UshortConverter>("u16");
        Add<uint, UintConverter>("u32");
        Add<ulong, UlongConverter>("u64");

        // Real types.
        Add<float, FloatConverter>("f32");
        Add<double, DoubleConverter>("f64");
        Add<decimal, DecimalConverter>("dec");

        // Char types.
        Add<char, CharConverter>("chr");

        // String types.
        Add<string, StringConverter>("str");
#if GODOT_CONTEXT
        Add<Godot.StringName, StringNameConverter>("GDsname");
        Add<Godot.NodePath, NodePathConverter>("GDnpath");
#endif

        // Time types.
        Add<DateTime, DateTimeConverter>("dt");

        // Binary types.
        Add<byte[], ByteArrayConverter>("u8[]");

        // Color types.
        Add<System.Drawing.Color, ColorConverter>("col");
#if GODOT_CONTEXT
        Add<Godot.Color, Converters.Gd.ColorConverter>("GDcol");
#endif

        // List types.
        Add(typeof(List<>), typeof(ListConverter<>), "list");
        Add(typeof(LinkedList<>), typeof(LinkedListConverter<>), "lnls");
        Add(typeof(HashSet<>), typeof(HashSetConverter<>), "hset");
        Add(typeof(Stack<>), typeof(StackConverter<>), "stck");
        Add(typeof(Queue<>), typeof(QueueConverter<>), "queu");
#if GODOT_CONTEXT
        Add(typeof(Godot.Collections.Array), typeof(Converters.Gd.ArrayConverter), "GDarr");
        Add(typeof(Godot.Collections.Array<>), typeof(Converters.Gd.ArrayConverter<>), "GDarrT");
        Add<Godot.Vector2, Converters.Gd.Vector2Converter>("GDv2f");
        Add<Godot.Vector3, Converters.Gd.Vector3Converter>("GDv3f");
        Add<Godot.Vector4, Converters.Gd.Vector4Converter>("GDv4f");
        Add<Godot.Vector2I, Converters.Gd.Vector2IConverter>("GDv2i");
        Add<Godot.Vector3I, Converters.Gd.Vector3IConverter>("GDv3i");
        Add<Godot.Vector4I, Converters.Gd.Vector4IConverter>("GDv4i");
        Add<Godot.Quaternion, Converters.Gd.QuaternionConverter>("GDquat");
        Add<Godot.Plane, Converters.Gd.PlaneConverter>("GDpln");
        Add<Godot.Rect2, Converters.Gd.Rect2Converter>("GDr2f");
        Add<Godot.Rect2I, Converters.Gd.Rect2IConverter>("GDr2i");
        Add<Godot.Aabb, Converters.Gd.AabbConverter>("GDaabb");
        Add<Godot.Transform2D, Converters.Gd.Transform2DConverter>("GDm2x2");
        Add<Godot.Basis, Converters.Gd.BasisConverter>("GDm3x3");
        Add<Godot.Transform3D, Converters.Gd.Transform3DConverter>("GDm3x4");
        Add<Godot.Projection, Converters.Gd.ProjectionConverter>("GDm4x4");
#endif

        // Dictionary types.
        Add(typeof(Dictionary<,>), typeof(DictionaryConverter<,>), "dict");
        Add(typeof(KeyValuePair<,>), typeof(KeyValuePairConverter<,>), "kvp");
#if GODOT_CONTEXT
        Add(typeof(Godot.Collections.Dictionary), typeof(Converters.Gd.DictionaryConverter), "GDdict");
        Add(typeof(Godot.Collections.Dictionary<,>), typeof(Converters.Gd.DictionaryConverter<,>), "GDdictT");
#endif

        // Generic types.
        Aliasses.Add(typeof(object), "obj");
        Aliasses.Add(typeof(Enum), "enum");
#if GODOT_CONTEXT
        Add(typeof(Godot.Variant), typeof(Converters.Gd.VariantConverter), "GDvar");
#endif
    }
}