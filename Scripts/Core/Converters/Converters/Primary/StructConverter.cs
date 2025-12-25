using System;
using System.Collections.Generic;
using System.Reflection;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A struct converter.
    /// </summary>
    public class StructConverter<T> : CompositeConverter<T, ObjectNode>
        where T : struct
    {
        /* Protected properties. */
        protected virtual HashSet<string> IgnoredMembers => new();

        /* Private properties. */
        private MemberInfo[] Members { get; set; }

        /* Protected methods. */
        protected override ObjectNode CreateNode(T obj, CreateNodeContext context)
        {
            // Collect members.
            if (Members == null)
                Members = GetPublicMembers(obj.GetType());

            // Create new node.
            return new(Members.Length);
        }

        protected override void AssignNode(ObjectNode node, T obj, AssignNodeContext context)
        {
            // Collect members.
            if (Members == null)
                Members = GetPublicMembers(obj.GetType());

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
                INode memberNode = context.CreateNode(memberType, memberValue);

                // Store finished identifier-node pair.
                node.Members[i] = new(memberstring, memberNode);
                memberNode.Parent = node;
            }
        }

        protected override void CollectTypes(ObjectNode node, CollectTypesContext context)
        {
            // Collect members.
            if (Members == null)
                Members = GetPublicMembers(typeof(T));

            // Collect member types.
            for (int i = 0; i < Members.Length; i++)
            {
                MemberInfo member = Members[i];
                if (member is FieldInfo field)
                    context.CollectTypes(node.Members[i].Value, field.FieldType);
                else if (member is PropertyInfo property)
                    context.CollectTypes(node.Members[i].Value, property.PropertyType);
            }
        }

        protected override T CreateObject(ObjectNode node, CreateObjectContext context)
        {
            // Collect members.
            if (Members == null)
                Members = GetPublicMembers(typeof(T));

            // Create object.
            object obj = new T();

            // Assign non-ref.
            for (int i = 0; i < Members.Length; i++)
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
                    object memberObj = context.CreateObject(field.FieldType, memberNode);
                    field.SetValue(obj, memberObj);
                }
                else if (member is PropertyInfo property)
                {
                    object memberObj = context.CreateObject(property.PropertyType, memberNode);
                    property.SetValue(obj, memberObj);
                }
            }

            return (T)obj;
        }

        protected override T FixReferences(T obj, ObjectNode node, FixReferencesContext context)
        {
            // Collect members.
            if (Members == null)
                Members = GetPublicMembers(typeof(T));

            object boxed = obj;

            // Assign non-ref.
            for (int i = 0; i < Members.Length; i++)
            {
                MemberInfo member = Members[i];

                // Match INode with member.
                string memberIdentifier = node.Members[i].Key;

                INode memberNode = node.Members[i].Value;

                if (memberIdentifier != member.Name)
                    throw new Exception($"Mismatch between members {i}: '{member.Name}' and '{memberIdentifier}'.");

                // If a ref node, get its value.
                if (memberNode is RefNode refNode)
                {
                    // Retrieve reference object.
                    object memberObj = context.GetReference(refNode.ID);

                    // Deconvert field/property.
                    if (member is FieldInfo field)
                        field.SetValue(boxed, memberObj);
                    else if (member is PropertyInfo property)
                        property.SetValue(boxed, memberObj);
                }

                // If not a ref node, fix member's references.
                else
                {
                    object memberObj = null;
                    if (member is FieldInfo field)
                    {
                        memberObj = field.GetValue(boxed);
                        memberObj = context.FixReferences(memberObj, memberNode);
                        field.SetValue(boxed, memberObj);
                    }
                    else if (member is PropertyInfo property)
                    {
                        memberObj = property.GetValue(boxed);
                        memberObj = context.FixReferences(memberObj, memberNode);
                        property.SetValue(boxed, memberObj);
                    }
                }
            }

            return (T)boxed;
        }

        /* Private methods. */
        /// <summary>
        /// Get all public members.
        /// </summary>
        private MemberInfo[] GetPublicMembers(Type type)
        {
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