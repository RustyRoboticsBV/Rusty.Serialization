using Rusty.Serialization.Nodes;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Rusty.Serialization.Serializers;

/// <summary>
/// A generic object serializer.
/// </summary>
public readonly struct ObjectSerializer<T> : ISerializer<T>
    where T : new()
{
    /* Fields. */
    private readonly MemberInfo[] members;

    /* Constructors. */
    public ObjectSerializer()
    {
        members = GetMembers(null);
    }

    /* Public methods. */
    public INode Serialize(T value, Registry context, bool addTypeLabel = false)
    {
        if (value == null)
            return new NullNode();

        var serialized = new KeyValuePair<Identifier, INode>[members.Length];

        for (int i = 0; i < serialized.Length; i++)
        {
            // Get member's info.
            MemberInfo member = members[i];

            // Get member value value.
            object memberValue = GetValue(value, member);

            // Serialize member into node.
            ISerializer serializer = context.GetSerializer(memberValue?.GetType());
            INode node = serializer.Serialize(memberValue, context);

            // Wrap in type node in case of type ambiguity.
            Type type = null;
            if (member is PropertyInfo property)
                type = property.PropertyType;
            else if (member is FieldInfo field)
                type = field.FieldType;
            if (memberValue != null && memberValue.GetType() != type)
                node = new TypeNode(context.GetTypeCode(memberValue.GetType()), node);

            // Store name-node pair.
            serialized[i] = new(member.Name, node);
        }

        return new ObjectNode(serialized);
    }

    public T Deserialize(INode node, Registry context)
    {
        if (node is NullNode @null && typeof(T).IsClass)
            return default;
        if (node is TypeNode type)
            return Deserialize(type.Object, context);
        if (node is ObjectNode obj)
        {
            T instance = new();

            // Collect member INodes as dictionary.
            var dict = new Dictionary<string, INode>(obj.Members.Length);
            for (int i = 0; i < obj.Members.Length; i++)
            {
                dict[obj.Members[i].Key] = obj.Members[i].Value;
            }

            // Deserialize members.
            foreach (var member in members)
            {
                if (dict.TryGetValue(member.Name, out INode valueNode))
                {
                    // Get serializer for the member type.
                    Type memberType = member switch
                    {
                        FieldInfo f => f.FieldType,
                        PropertyInfo p => p.PropertyType,
                        _ => throw new InvalidOperationException($"Unsupported member type '{member.MemberType}'.")
                    };

                    // If the value node was a type node, unpack it and find its type.
                    if (valueNode is TypeNode typeNode)
                    {
                        memberType = context.FindType(typeNode.TypeCode);
                        valueNode = typeNode.Object;
                        if (memberType == null)
                            throw new Exception("Could not resolve type code " + typeNode.TypeCode);
                    }

                    // Get serializer.
                    ISerializer serializer = context.GetSerializer(memberType);

                    // Deserialize member.
                    object value = serializer.Deserialize(valueNode, context);

                    // Store value.
                    SetValue(instance, member, value);
                }
            }

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

    private static MemberInfo[] GetMembers(Identifier[] memberNames)
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

        var fields = typeof(T).GetFields(flags).Cast<MemberInfo>();
        var props = typeof(T).GetProperties(flags)
            .Where(p => p.CanRead && p.CanWrite)
            .Cast<MemberInfo>();

        var allMembers = fields.Concat(props);

        if (memberNames != null)
            return allMembers.Where(m => memberNames.Any(id => (string)id == m.Name)).ToArray();

        return allMembers.ToArray();
    }
}