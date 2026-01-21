using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Rusty.Serialization.Core.Nodes;


#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif
#if GODOT
using Godot;
#endif

namespace Rusty.Serialization.Core.Conversion
{
    /// <summary>
    /// A struct/class converter.
    /// </summary>
    public class ClassConverter<T> : CompositeConverter<T, ObjectNode>
    {
        /* Protected properties. */
        /// <summary>
        /// A set of members that should not be serialized.
        /// </summary>
        protected virtual HashSet<MemberInfo> IgnoredMembers => new HashSet<MemberInfo>();

        /* Private properties. */
        private MemberInfo[] Members { get; set; }

        /* Protected methods. */
        protected override ObjectNode CreateNode(T obj, CreateNodeContext context)
        {
            // Collect members.
            Members = CollectMembers(obj.GetType());

            // Create new node.
            return new(Members.Length);
        }

        protected override void AssignNode(ObjectNode node, T obj, AssignNodeContext context)
        {
            // Collect members.
            if (Members == null)
                Members = CollectMembers(obj.GetType());

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
            Members = CollectMembers(typeof(T));

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

        protected override T CreateObject(ObjectNode node, CreateObjectContext context) => (T)Activator.CreateInstance(typeof(T));

        protected override T AssignObject(T obj, ObjectNode node, AssignObjectContext context)
        {
            // Collect members.
            Members = CollectMembers(typeof(T));

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
                    object memberObj = context.CreateChildObject(field.FieldType, memberNode);
                    field.SetValue(obj, memberObj);
                }
                else if (member is PropertyInfo property)
                {
                    object memberObj = context.CreateChildObject(property.PropertyType, memberNode);
                    property.SetValue(obj, memberObj);
                }
            }

            return obj;
        }

        /* Private methods. */
        /// <summary>
        /// Get all serializable members.
        /// </summary>
        private MemberInfo[] CollectMembers(Type type)
        {
            // Do nothing if we already got the members.
            if (Members != null)
                return Members;

            // Collect fields.
            List<FieldInfo> fields = new List<FieldInfo>();
            CollectFields(type, fields, IgnoredMembers);

            // Collect properties with a public getter and setter.
            List<PropertyInfo> properties = new List<PropertyInfo>();
            CollectProperties(type, properties, fields, IgnoredMembers);

            // Combine collected members.
            List<MemberInfo> members = new List<MemberInfo>(fields.Count + properties.Count);

            for (int i = 0; i < fields.Count; i++)
            {
                members.Add(fields[i]);
            }

            for (int i = 0; i < properties.Count; i++)
            {
                members.Add(properties[i]);
            }

            return members.ToArray();
        }

        /// <summary>
        /// Collect all fields that should be serialized.
        /// </summary>
        private static void CollectFields(Type type, List<FieldInfo> members, HashSet<MemberInfo> ignoredMembers)
        {
            // Stop on System.Object or null.
            if (type == typeof(object) || type == null)
                return;

            // Get all fields.
            FieldInfo[] fields = type.GetFields(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.DeclaredOnly);

            // Only keep the ones that match the serializable requirements.
            for (int i = 0; i < fields.Length; i++)
            {
                // Skip ignored or [NonSerialized].
                if (fields[i].GetCustomAttribute<NonSerializedAttribute>() != null || ignoredMembers.Contains(fields[i]))
                    continue;

                // Check if this property is serializable.
                if (fields[i].IsPublic)
                    members.Add(fields[i]);

#if NETFRAMEWORK || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
                else if (fields[i].GetCustomAttribute<DataMemberAttribute>() != null)
                    members.Add(fields[i]);
#endif
#if UNITY_5_3_OR_NEWER
                else if (fields[i].GetCustomAttribute<SerializeField>() != null)
                    members.Add(fields[i]);
#endif
#if UNITY_2019_3_OR_NEWER
                else if (fields[i].GetCustomAttribute<SerializeReference>() != null)
                    members.Add(fields[i]);
#endif
#if GODOT
                else if (fields[i].GetCustomAttribute<ExportAttribute>() != null)
                    members.Add(fields[i]);
#endif
            }

            // Examine base type.
            CollectFields(type.BaseType, members, ignoredMembers);
        }

        /// <summary>
        /// Collect all properties that should be serialized.
        /// </summary>
        private static void CollectProperties(Type type, List<PropertyInfo> members, List<FieldInfo> collectedFields,
            HashSet<MemberInfo> ignoredMembers)
        {
            // Stop on System.Object or null.
            if (type == typeof(object) || type == null)
                return;

            // Get all properties.
            PropertyInfo[] properties = type.GetProperties(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.DeclaredOnly);

            // Only keep the ones that match the serializable requirements.
            for (int i = 0; i < properties.Length; i++)
            {
                // Get property info.
                PropertyInfo property = properties[i];

                MethodInfo getter = property.GetGetMethod(true);
                MethodInfo setter = property.GetSetMethod(true);

                // Skip indexers.
                if (property.GetIndexParameters().Length != 0)
                    continue;

                // Skip ignored or [NonSerialized].
                if (ignoredMembers.Contains(property) || property.GetCustomAttribute<NonSerializedAttribute>() != null)
                    continue;

                // Skip static properties.
                if ((getter != null && getter.IsStatic) ||
                    (setter != null && setter.IsStatic))
                    continue;

                // Check if this property is serializable.
                bool isSerializable = false;

                if (getter != null && getter.IsPublic && setter != null && setter.IsPublic)
                    isSerializable = true;

#if NETFRAMEWORK || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
                else if (property.GetCustomAttribute<DataMemberAttribute>() != null)
                    isSerializable = true;
#endif
#if GODOT
                else if (property.GetCustomAttribute<ExportAttribute>() != null)
                    isSerializable = true;
#endif

                if (!isSerializable)
                    continue;

                // Avoid duplicates if a backing field is already serialized.
                for (int j = 0; j < collectedFields.Count; j++)
                {
                    if (collectedFields[j].Name == $"<{property.Name}>k__BackingField")
                        continue;
                }

                members.Add(property);
            }

            // Examine base type
            CollectProperties(type.BaseType, members, collectedFields, ignoredMembers);
        }
    }
}