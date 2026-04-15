using System;
using System.Collections.Generic;
using System.Reflection;
using Rusty.Serialization.Core.Nodes;

#if NETFRAMEWORK || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
using System.Runtime.Serialization;
#endif
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
    public class ClassConverter<T> : Converter
    {
        /* Public types. */
        public struct MemberName
        {
            /* Public properties. */
            public string Scope { get; private set; }
            public string Name { get; private set; }

            /* Constructors. */
            public MemberName(string scope, string name)
            {
                Scope = scope;
                Name = name;
            }

            public MemberName(string name) : this(null, name) { }

            /* Public methods. */
            public override string ToString()
            {
                if (Scope == null)
                    return Name;
                else
                    return Scope + '.' + Name;
            }
        }

        /* Protected properties. */
        /// <summary>
        /// A set of members that should not be serialized.
        /// </summary>
        protected virtual HashSet<MemberInfo> IgnoredMembers => new HashSet<MemberInfo>();

        /* Private properties. */
        private MemberInfo[] Members { get; set; }
        private Dictionary<MemberName, MemberInfo> MemberLookup { get; set; } = new Dictionary<MemberName, MemberInfo>();

        /* Public methods. */
        public override ObjectNode CreateNode(object obj, CreateNodeContext context)
        {
            // Collect members.
            CollectMembers();

            // Create new node.
            return new ObjectNode(Members.Length);
        }

        /* Protected methods. */
        protected override void PopulateNode(object obj, ObjectNode node, PopulateNodeContext context)
        {
            // Collect members.
            CollectMembers();

            // Convert identifiers and convert values to member nodes.
            Type type = typeof(T);
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
                SymbolNode symbol = new SymbolNode(member.Name);

                ScopeNode scope = null;
                if (member.DeclaringType != type)
                    scope = new ScopeNode(member.DeclaringType.FullName, symbol);

                IMemberNameNode memberName = scope != null ? scope : symbol;

                // Create member node.
                INode memberNode = context.CreateNode(memberType, memberValue);

                // Store finished identifier-node pair.
                node.Members[i] = new KeyValuePair<IMemberNameNode, INode>(memberName, memberNode);
                memberNode.Parent = node;
            }
        }

        protected override void CollectChildNodeTypes(ObjectNode node, CollectTypesContext context)
        {
            // Collect members.
            CollectMembers();

            // Collect member types.
            for (int i = 0; i < node.Count; i++)
            {
                // Find member.
                MemberName memberName = GetMemberName(node.GetNameAt(i));
                if (!MemberLookup.TryGetValue(memberName, out MemberInfo member))
                {
                    throw new MemberAccessException($"The type '{typeof(T)}' has no member named '{memberName}'.\n");
                }

                // Collect member type.
                if (member is FieldInfo field)
                    context.Collect(node.Members[i].Value, field.FieldType);
                else if (member is PropertyInfo property)
                    context.Collect(node.Members[i].Value, property.PropertyType);
            }
        }

        protected override object CreateObject(ObjectNode node, CreateObjectContext context)
            => Activator.CreateInstance(typeof(T));

        protected override object PopulateObject(ObjectNode node, object obj, PopulateObjectContext context)
        {
            // Collect members.
            CollectMembers();

            // Assign non-ref.
            for (int i = 0; i < node.Count; i++)
            {
                // Find member.
                MemberName memberName = GetMemberName(node.GetNameAt(i));
                if (!MemberLookup.TryGetValue(memberName, out MemberInfo member))
                    throw new MemberAccessException($"The type '{typeof(T)}' has no member named '{memberName}'.");

                // Deconvert field/property.
                INode valueNode = node.GetValueAt(i);
                if (member is FieldInfo field)
                {
                    object memberObj = context.CreateChildObject(valueNode, field.FieldType);
                    field.SetValue(obj, memberObj);
                }
                else if (member is PropertyInfo property)
                {
                    object memberObj = context.CreateChildObject(valueNode, property.PropertyType);
                    property.SetValue(obj, memberObj);
                }
            }

            return obj;
        }

        /* Private methods. */
        /// <summary>
        /// Get all serializable members.
        /// </summary>
        private void CollectMembers()
        {
            // Do nothing if we already got the members.
            if (Members != null)
                return;

            // Collect fields.
            Type type = typeof(T);

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

            Members = members.ToArray();

            // Create lookup dictionary.
            MemberLookup.Clear();
            for (int i = 0; i < members.Count; i++)
            {
                string scope = members[i].DeclaringType != type ? members[i].DeclaringType.FullName : null;
                string name = members[i].Name;
                MemberLookup.Add(new MemberName(scope, name), members[i]);
            }
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
                if ((getter != null && getter.IsStatic) || (setter != null && setter.IsStatic))
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
                        goto Skip;
                }

                members.Add(property);

                Skip: { }
            }

            // Examine base type
            CollectProperties(type.BaseType, members, collectedFields, ignoredMembers);
        }

        /// <summary>
        /// Read a member name node.
        /// </summary>
        private static MemberName GetMemberName(INode node)
        {
            if (node is ScopeNode scope)
                return new MemberName(scope.Name, scope.Child.Name);
            else if (node is SymbolNode symbol)
                return new MemberName(symbol.Name);
            else
                throw new ArgumentException("Invalid member name node: " + node);
        }
    }
}