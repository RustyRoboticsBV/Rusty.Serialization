using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rusty.Serialization
{
    public struct FullTypeName
    {
        /* Fields. */
        private readonly string @namespace;
        private readonly FullTypeName[] declaringTypes;
        private readonly string name;
        private readonly FullTypeName[] genericArguments;

        private readonly int hashcode;

        /* Constructors. */
        public FullTypeName(Type type)
        {
            // Generic parameter types.
            if (type.IsGenericParameter)
            {
                @namespace = "";
                declaringTypes = Array.Empty<FullTypeName>();
                name = type.Name;
                genericArguments = Array.Empty<FullTypeName>();
                hashcode = ToString().GetHashCode();
                return;
            }

            // Store namespace.
            @namespace = type.Namespace;

            // Build declaring type chain.
            List<Type> chain = new();
            Type current = type.DeclaringType;
            while (current != null)
            {
                chain.Insert(0, current);
                current = current.DeclaringType;
            }

            // Flattened generic arguments.
            Type[] allArgs = type.IsGenericType ? type.GetGenericArguments() : [];
            int argIndex = 0;

            List<FullTypeName> declaringChain = new();
            foreach (var t in chain)
            {
                int ownArgsCount = t.IsGenericType ? t.GetGenericArguments().Length : 0;
                FullTypeName[] ownArgs = new FullTypeName[ownArgsCount];
                for (int i = 0; i < ownArgsCount; i++)
                {
                    ownArgs[i] = new FullTypeName(allArgs[argIndex++]);
                }

                declaringChain.Add(new(t.Namespace, [], GetTypeName(t), ownArgs));
            }

            declaringTypes = declaringChain.ToArray();

            // Get own generic arguments.
            int ownCount = type.IsGenericType ? type.GetGenericArguments().Length - argIndex : 0;
            genericArguments = new FullTypeName[ownCount];
            for (int i = 0; i < ownCount; i++)
            {
                genericArguments[i] = new FullTypeName(allArgs[argIndex++]);
            }

            // Get name.
            var attribute = type.GetCustomAttribute<SerializableAttribute>();
            if (attribute != null)
            {
                name = attribute.Name;
                @namespace = "";
            }
            else
                name = GetTypeName(type);
            hashcode = ToString().GetHashCode();
        }

        private FullTypeName(string ns, FullTypeName[] declaringTypes, string name, FullTypeName[] genericArguments)
        {
            @namespace = ns;
            this.declaringTypes = declaringTypes;
            this.name = name;
            this.genericArguments = genericArguments;
            hashcode = ToString().GetHashCode();
        }

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

            // Main type name.
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
        private static string GetTypeName(Type type)
        {
            string name = type.Name;

            // Remove backtick notation if present.
            int backtick = name.IndexOf('`');
            if (backtick >= 0)
                name = name.Substring(0, backtick);
            return name;
        }
    }
}
