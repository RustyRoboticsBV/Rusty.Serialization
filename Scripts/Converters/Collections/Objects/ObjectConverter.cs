using System;
using System.Collections.Generic;
using System.Reflection;
using Rusty.Serialization.Nodes;

namespace Rusty.Serialization.Converters;

/// <summary>
/// A generic struct or class converter.
/// </summary>
public abstract class ObjectConverter<T> : IConverter<T>
    where T : new()
{
    /* Protected methods. */
    public virtual INode Convert(T obj, Context context)
    {
        // Collect public members.
        MemberInfo[] members = GetPublicMembers(obj);

        // Collect identifiers and convert values to member nodes.
        var memberPairs = new KeyValuePair<Identifier, INode>[members.Length];
        for (int i = 0; i < members.Length; i++)
        {
            MemberInfo member = members[i];

            Identifier identifier = member.Name;

            INode node = null;
            if (member is FieldInfo field)
                node = ConvertMember(field.GetValue(obj), context);
            else if (member is PropertyInfo property)
                node = ConvertMember(property.GetValue(obj), context);

            memberPairs[i] = new(identifier, node);
        }

        // Return finished object node.
        return new ObjectNode(memberPairs);
    }

    public virtual T Deconvert(INode node, Context context)
    {
        if (node is ObjectNode objNode)
        {
            // Create new object.
            T obj = new();

            // Collect public members.
            Type type = obj.GetType();
            MemberInfo[] members = GetPublicMembers(obj);

            // Set all members that have values in the node.
            for (var i = 0; i < objNode.Members.Length; i++)
            {
                MemberInfo member = members[i];

                Identifier memberIdentifier = objNode.Members[i].Key;

                INode memberNode = objNode.Members[i].Value;
                if (memberIdentifier != member.Name)
                    throw new Exception($"Mismatch between members {i}: '{member.Name}' and '{memberIdentifier}'.");

                object memberObj = DeconvertMember(memberNode, context);
                if (member is FieldInfo field)
                    field.SetValue(obj, memberObj);
                else if (member is PropertyInfo property)
                    property.SetValue(obj, memberObj);
            }
            return obj;
        }
        throw new ArgumentException($"Cannot deconvert nodes of type '{node.GetType()}'.");
    }

    /* Private methods. */
    private static MemberInfo[] GetPublicMembers(T obj)
    {
        Type type = obj.GetType();
        List<MemberInfo> members = new();

        // Collect public fields.
        FieldInfo[] fields = type.GetFields();
        for (int i = 0; i < fields.Length; i++)
        {
            if (fields[i].IsPublic)
                members.Add(fields[i]);
        }

        // Collect properties with a public getter and setter.
        PropertyInfo[] properties = type.GetProperties();
        for (int i = 0; i < properties.Length; i++)
        {
            if (properties[i].GetMethod.IsPublic && properties[i].SetMethod.IsPublic)
                members.Add(properties[i]);
        }

        return members.ToArray();
    }

    /// <summary>
    /// Convert a member into a node.
    /// </summary>
    private INode ConvertMember<U>(U obj, Context context)
    {
        return context.GetConverter(obj.GetType()).Convert(obj, context);
    }

    /// <summary>
    /// Deconvert a member node into an object.
    /// </summary>
    private object DeconvertMember(INode node, Context context)
    {
        Type targetType = ((IConverter)this).TargetType;
        return context.GetConverter(targetType).Deconvert(node, context);
    }
}