using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SysColor = System.Drawing.Color;
using Godot;

#if GODOT
using GdColor = Godot.Color;
#elif UNITY_5_OR_NEWER
using UColor = UnityEngine.Color;
using UColor32 = UnityEngine.Color32;
#endif

namespace Rusty.Serialization.Nodes;

/// <summary>
/// An utility class for converting C# objects to serialized data.
/// </summary>
public static class ObjectConversionUtility
{
    /// <summary>
    /// Converts a C# object into an Object-node, including only the specified fields/properties.
    /// </summary>
    public static Object Convert(object obj, string typeCode, params string[] memberNames)
    {
        // Throw if the object is null.
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        // Handle null type codes.
        Type type = obj.GetType();
        if (typeCode == null)
            typeCode = type.Name;

        // Parse members.
        List<KeyValuePair<string, INode>> members = new();

        foreach (string name in memberNames)
        {
            MemberInfo member =
                type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public) as MemberInfo ??
                type.GetField(name, BindingFlags.Instance | BindingFlags.Public);

            if (member == null)
                throw new ArgumentException($"Type {type.Name} does not contain public field or property '{name}'.");

            object value = member switch
            {
                PropertyInfo p => p.GetValue(obj),
                FieldInfo f => f.GetValue(obj),
                _ => null
            };

            INode nodeVal = ToNode(value);
            members.Add(new(name, nodeVal));
        }

        return CreateObjectNode(typeCode, members);
    }

    /// <summary>
    /// Converts any .NET value to the correct serializer node type.
    /// </summary>
    public static INode ToNode(object value)
    {
        // Null.
        if (value == null)
            return String.Deserialize("null");

        Type t = value.GetType();

        // Booleans.
        if (value is bool b)
            return (Boolean)b;

        // Integers.
        if (value is sbyte @sbyte)
            return (Integer)@sbyte;
        if (value is byte @byte)
            return (Integer)@byte;
        if (value is short @short)
            return (Integer)@short;
        if (value is ushort @ushort)
            return (Integer)@ushort;
        if (value is int @int)
            return (Integer)@int;
        if (value is uint @uint)
            return (Integer)@uint;
        if (value is long @long)
            return (Integer)@long;
        if (value is ulong @ulong)
            return (Integer)@ulong;

        // Floats.
        if (value is float @float)
            return (Float)@float;
        if (value is double @double)
            return (Float)@double;
        if (value is decimal @decimal)
            return (Float)@decimal;

        // Characters.
        if (value is char ch)
            return (Character)ch;

        // Strings.
        if (value is string str)
            return (String)str;
#if GODOT
        if (value is StringName name)
            return (String)name;
#endif

        // Colors.
        if (value is SysColor col)
            return new Color(col);
#if GODOT
        if (value is GdColor gcol)
            return new Color(gcol);
#elif UNITY_5_OR_NEWER
        if (value is UColor ucol)
            return new Color(ucol);
        if (value is UColor32 ucol32)
            return new Color(ucol32);
#endif

        // Lists.
        if (value is IList list)
        {
            List<INode> elements = new();
            foreach (object elem in list)
            {
                elements.Add(ToNode(elem));
            }
            return new List(elements);
        }

        // Dictionaries.
        if (value is IDictionary dict)
        {
            List<KeyValuePair<INode, INode>> pairs = new();

            foreach (DictionaryEntry entry in dict)
            {
                INode key = ToNode(entry.Key);
                INode val = ToNode(entry.Value);
                pairs.Add(new KeyValuePair<INode, INode>(key, val));
            }

            return new Dictionary(pairs.ToArray());
        }

        // Fallback: treat it as a structured object
        return Convert(value, null, GetAllSerializableMembers(t));
    }

    /// <summary>
    /// Creates an object node.
    /// </summary>
    private static Object CreateObjectNode(string typeName, List<KeyValuePair<string, INode>> members)
    {
        return new Object(
            typeName,
            members.ToArray()
        );
    }

    /// <summary>
    /// When recursively converting unknown objects, automatically select all public fields + properties.
    /// </summary>
    private static string[] GetAllSerializableMembers(Type t)
    {
        var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public)
                      .Select(f => f.Name);
        var props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                      .Select(p => p.Name);

        return fields.Concat(props).ToArray();
    }
}