using Rusty.Serialization.Nodes;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Rusty.Serialization;

/// <summary>
/// A generic object serializer.
/// </summary>
public readonly struct ObjectSerializer<T> : ISerializer<T>
    where T : class, new()
{
    /* Fields. */
    private readonly string typeCode;
    private readonly MemberInfo[] members;

    /* Constructors. */
    public ObjectSerializer(string typeCode = null, params string[] memberNames)
    {
        this.typeCode = typeCode ?? typeof(T).Name;
        members = GetMembers(memberNames) ?? [];

        System.Console.WriteLine(members);
    }

    /* Public methods. */
    public INode Serialize(T value, Registry context)
    {
        if (value == null)
            return new NullNode();

        var serialized = new KeyValuePair<string, INode>[members.Length];

        for (int i = 0; i < serialized.Length; i++)
        {
            MemberInfo member = members[i];

            object rawValue = GetValue(value, member);

            ISerializer serializer = context.GetSerializer(rawValue.GetType());
            INode node = serializer.Serialize(rawValue, context);

            serialized[i] = new(member.Name, node);
        }

        return new ObjectNode(typeCode, serialized);
    }

    public T Deserialize(INode node, Registry context)
    {
        if (node is NullNode @null)
            return null;
        else if (node is ObjectNode obj)
        {
            T instance = new();
            // TODO
            return instance;

        }
        throw new ArgumentException($"'{GetType()}' cannot deserialize node of type '{node.GetType()}'.");
    }

    /* Private methods. */
    private static object GetValue(T obj, MemberInfo member)
    {
        return member switch
        {
            FieldInfo f => f.GetValue(obj),
            PropertyInfo p => p.GetValue(obj),
            _ => throw new InvalidOperationException($"Unsupported member type {member.MemberType}.")
        };
    }

    private static void SetValue(T obj, MemberInfo member, object value)
    {
        switch (member)
        {
            case FieldInfo f:
                f.SetValue(obj, value);
                break;
            case PropertyInfo p:
                p.SetValue(obj, value);
                break;
            default:
                throw new InvalidOperationException($"Unsupported member type {member.MemberType}.");
        }
    }

    private static MemberInfo[] GetMembers(string[] memberNames)
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

        var fields = typeof(T).GetFields(flags).Cast<MemberInfo>();
        var props = typeof(T).GetProperties(flags)
            .Where(p => p.CanRead && p.CanWrite)
            .Cast<MemberInfo>();

        var allMembers = fields.Concat(props);

        if (memberNames != null)
            return allMembers.Where(m => memberNames.Contains(m.Name)).ToArray();

        return allMembers.ToArray();
    }
}