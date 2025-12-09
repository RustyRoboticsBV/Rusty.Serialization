using Rusty.Serialization.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A generic struct converter.
    /// </summary>
    public class StructConverter<T> : ValueConverter<T, ObjectNode>
        where T : struct
    {
        /* Protected properties. */
        protected virtual HashSet<string> IgnoredMembers => new();

        /* Private properties. */
        private MemberInfo[] Members { get; set; }

        /* Protected methods. */
        protected override ObjectNode ConvertValue(T obj, IConverterScheme scheme, SymbolTable table)
        {
            // Collect members.
            if (Members == null)
                Members = GetPublicMembers(obj);

            // Create new node.
            ObjectNode node = new(Members.Length);

            // Convert identifiers and convert values to member nodes.
            for (int i = 0; i < Members.Length; i++)
            {
                MemberInfo member = Members[i];

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
                INode memberNode = ConvertNested(memberType, memberValue, scheme, table);

                // Store finished identifier-node pair.
                node.Members[i] = new(memberstring, memberNode);
                memberNode.Parent = node;
            }
            return node;
        }

        protected override T DeconvertValue(ObjectNode node, IConverterScheme scheme, ParsingTable table)
        {
            // Create new object.
            T obj = new();

            // Collect members.
            if (Members == null)
                Members = GetPublicMembers(obj);

            // Set all members that have values in the node.
            for (var i = 0; i < node.Members.Length; i++)
            {
                MemberInfo member = Members[i];

                // Match INode with member.
                string memberIdentifier = node.Members[i].Key;

                INode memberNode = node.Members[i].Value;
                if (memberIdentifier != member.Name)
                    throw new Exception($"Mismatch between members {i}: '{member.Name}' and '{memberIdentifier}'.");

                // Deconvert field/property.
                if (member is FieldInfo field)
                {
                    object memberObj = DeconvertNested(field.FieldType, memberNode, scheme, table);
                    field.SetValue(obj, memberObj);
                }
                else if (member is PropertyInfo property)
                {
                    object memberObj = DeconvertNested(property.PropertyType, memberNode, scheme, table);
                    property.SetValue(obj, memberObj);
                }
            }

            return obj;
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