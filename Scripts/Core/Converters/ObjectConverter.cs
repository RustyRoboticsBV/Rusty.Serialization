using System;
using System.Collections.Generic;
using System.Reflection;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic struct or class converter.
    /// </summary>
    public abstract class ObjectConverter<T> : Converter<T>
        where T : new()
    {
        /* Protected properties. */
        protected virtual HashSet<string> IgnoredMembers => new();

        /* Protected methods. */
        public override INode Convert(T obj, IConverterScheme scheme)
        {
            // Collect public members.
            MemberInfo[] members = GetPublicMembers(obj);

            // Create node.
            ObjectNode node = new(members.Length);

            // Collect identifiers and convert values to member nodes.
            for (int i = 0; i < members.Length; i++)
            {
                MemberInfo member = members[i];

                // Collect member type and member value.
                Type memberType = null;
                object memberValue = null;

                if (member is FieldInfo field)
                {
                    memberType = field.FieldType;
                    memberValue = field.GetValue(obj);
                }
                else if (member is PropertyInfo property)
                {
                    memberType = property.PropertyType;
                    memberValue = property.GetValue(obj);
                }

                // Get member identifier.
                string memberstring = member.Name;

                // Create member node.
                INode memberNode = ConvertNested(memberType, memberValue, scheme);

                // Store finished identifier-node pair.
                node.Members[i] = new(memberstring, memberNode);
                memberNode.Parent = node;
            }

            return node;
        }

        public override T Deconvert(INode node, IConverterScheme scheme)
        {
            if (node is TypeNode typeNode)
                return Deconvert(typeNode.Value, scheme);
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

                    string memberIdentifier = objNode.Members[i].Key;

                    INode memberNode = objNode.Members[i].Value;
                    if (memberIdentifier != member.Name)
                        throw new Exception($"Mismatch between members {i}: '{member.Name}' and '{memberIdentifier}'.");

                    if (member is FieldInfo field)
                    {
                        object memberObj = DeconvertNested(field.FieldType, memberNode, scheme);
                        field.SetValue(obj, memberObj);
                    }
                    else if (member is PropertyInfo property)
                    {
                        object memberObj = DeconvertNested(property.PropertyType, memberNode, scheme);
                        property.SetValue(obj, memberObj);
                    }
                }
                return obj;
            }
            throw new ArgumentException($"{GetType().Name} cannot deconvert nodes of valueType '{node.GetType()}'.");
        }

        /* Private methods. */
        private MemberInfo[] GetPublicMembers(T obj)
        {
            Type type = obj.GetType();
            List<MemberInfo> members = new();

            // Collect public fields.
            FieldInfo[] fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].IsPublic && !fields[i].IsStatic && !IgnoredMembers.Contains(fields[i].Name))
                    members.Add(fields[i]);
            }

            // Collect properties with a public getter and setter.
            PropertyInfo[] properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                MethodInfo getter = properties[i].GetMethod;
                MethodInfo setter = properties[i].SetMethod;
                if (properties[i].GetIndexParameters().Length == 0 && !IgnoredMembers.Contains(properties[i].Name)
                    && getter != null && getter.IsPublic && !getter.IsStatic
                    && setter != null && setter.IsPublic && !setter.IsStatic)
                {
                    members.Add(properties[i]);
                }
            }

            return members.ToArray();
        }
    }
}