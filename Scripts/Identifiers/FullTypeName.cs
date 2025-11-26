using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rusty.Serialization
{
    /// <summary>
    /// Represents a full type name, including namespace, declaring types, and generic arguments.
    /// </summary>
    public struct FullTypeName
    {
        /* Fields. */
        private readonly TypeName @namespace;
        private readonly FullTypeName[] declaringTypes;
        private readonly TypeName name;
        private readonly FullTypeName[] genericArguments;

        private readonly int hashcode;

        /* Constructors. */
        public FullTypeName(Type type)
        {
            if (type.IsGenericParameter)
            {
                @namespace = "";
                declaringTypes = [];
                name = type.Name;
                genericArguments = [];
            }

            else
            {
                @namespace = type.Namespace;
                declaringTypes = GetDeclaringTypeChain(type);
                name = GetTypeName(type);
                genericArguments = GetOwnGenericArguments(type);
                hashcode = ToString().GetHashCode();
            }
        }

        /* Conversion operators. */
        public static implicit operator string(FullTypeName typeName)
        {
            StringBuilder sb = new StringBuilder();

            // Namespace.
            if (!string.IsNullOrEmpty(typeName.@namespace))
            {
                sb.Append(typeName.@namespace);
                sb.Append('.');
            }

            // Declaring types.
            for (int i = 0; i < typeName.declaringTypes.Length; i++)
            {
                sb.Append(typeName.declaringTypes[i]);
                sb.Append('+');
            }

            // Main type.
            sb.Append(typeName.name);

            // Generic arguments.
            if (typeName.genericArguments.Length > 0)
            {
                sb.Append('<');
                for (int i = 0; i < typeName.genericArguments.Length; i++)
                {
                    if (i > 0)
                        sb.Append(",");
                    sb.Append(typeName.genericArguments[i]);
                }
                sb.Append('>');
            }

            return sb.ToString();
        }

        public static implicit operator TypeName(FullTypeName typeName) => (string)typeName;

        /* Public methods. */
        public override string ToString() => (string)this;
        public override int GetHashCode() => hashcode;

        /* Private methods. */
        /// <summary>
        /// Get a type's declaring type chain.
        /// </summary>
        private static FullTypeName[] GetDeclaringTypeChain(Type type)
        {
            if (!type.IsNested)
                return [];

            // Walk up the declaring type chain and collect full names.
            List<FullTypeName> chain = new();
            while (type.DeclaringType != null)
            {
                type = type.DeclaringType;
                chain.Add(new(type));
            }
            return chain.ToArray();
        }

        /// <summary>
        /// Get the name of a type.
        /// </summary>
        private static TypeName GetTypeName(Type type)
        {
            // Check attribute.
            var attribute = type.GetCustomAttribute<SerializableAttribute>();
            if (attribute != null)
                return attribute.Name;

            // Otherwise, infer name from type.
            string name = type.Name;

            // Remove backtick notation.
            int backtick = name.IndexOf('`');
            if (backtick >= 0)
                name = name.Substring(0, backtick);

            return new(name);
        }

        /// <summary>
        /// Returns the generic type arguments that belong to a single type (excluding outer declaring types).
        /// </summary>
        private static FullTypeName[] GetOwnGenericArguments(Type type)
        {
            if (!type.IsGenericType)
                return [];

            // Get all generic arguments.
            Type[] allArgs = type.GetGenericArguments();

            // Get the number of generic arguments in the declaring type.
            int outerCount = 0;
            if (type.DeclaringType != null && type.DeclaringType.IsGenericType)
                outerCount = type.DeclaringType.GetGenericTypeDefinition().GetGenericArguments().Length;

            // Get our own generic types.
            int ownCount = allArgs.Length - outerCount;
            Type[] ownArgs = new Type[ownCount];
            for (int i = 0; i < ownCount; i++)
            {
                ownArgs[i] = allArgs[outerCount + i];
            }

            FullTypeName[] names = new FullTypeName[ownCount];
            for (int i = 0; i < ownCount; i++)
            {
                names[i] = new(ownArgs[i]);
            }
            return names;
        }
    }
}
