using Rusty.Serialization.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// Represents a type name.
    /// </summary>
    public struct TypeName
    {
        /* Fields. */
        private string nameSpace;
        private string name;
        private TypeName[] genericArgs;
        private string arraySuffix;

        /* Constructors. */
        public TypeName(Type type)
        {
            nameSpace = "";
            name = "";
            genericArgs = new TypeName[0];
            arraySuffix = "";
            ParseType(type);
        }

        public TypeName(string typeName)
        {
            var result = TypeNameParser.Parse(typeName);

            nameSpace = result.Namespace;
            name = result.Name;
            genericArgs = new TypeName[result.GenericArgs.Count];
            for (int i = 0; i < genericArgs.Length; i++)
            {
                genericArgs[i] = new(result.GenericArgs[i]);
            }
            arraySuffix = result.ArraySuffix;
        }

        /* Conversion operators. */
        public static implicit operator Type(TypeName tn) => tn.ToType();
        public static implicit operator string(TypeName tn) => tn.ToString();

        /* Public methods. */
        public override readonly string ToString()
        {
            StringBuilder sb = new();

            // Add namespace.
            if (nameSpace.Length > 0)
            {
                sb.Append(nameSpace);
                sb.Append('.');
            }

            // Add type.
            sb.Append(name);

            // Add generic type arguments.
            if (genericArgs.Length > 0)
            {
                sb.Append("[");
                for (int i = 0; i < genericArgs.Length; i++)
                {
                    if (i > 0)
                        sb.Append(',');
                    sb.Append(genericArgs[i]);
                }
                sb.Append("]");
            }

            // Add array suffix.
            sb.Append(arraySuffix);

            return sb.ToString();
        }

        public Type ToType()
        {
            string typeName = ToString();

            // 1. Try standard Type.GetType first (works for system types)
            Type type = Type.GetType(typeName);
            if (type != null)
                return type;

            // 2. Search all currently loaded assemblies.
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(typeName);
                if (type != null)
                    return type;
            }

            // 3. As a last resort, search by name only (ignores namespace conflicts).
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetTypes().FirstOrDefault(t => t.FullName == typeName || t.Name == typeName);
                if (type != null)
                    return type;
            }

            return null; // not found
        }

        public override readonly int GetHashCode()
        {
            int hashcode = ((nameSpace.GetHashCode() ^ 11) + name.GetHashCode()) ^ 11;
            foreach (TypeName type in genericArgs)
            {
                hashcode = hashcode ^ 11 + type.GetHashCode();
            }
            return hashcode;
        }

        /* Private methods. */
        private void ParseType(Type type)
        {
            // Handle null.
            if (type == null)
            {
                nameSpace = "";
                name = "null";
                genericArgs = new TypeName[0];
                return;
            }

            // Handle generic type parameters.
            if (type.IsGenericParameter)
            {
                nameSpace = "";
                name = type.Name;
                genericArgs = new TypeName[0];
                return;
            }

            // Namespace.
            nameSpace = type.Namespace ?? "";

            // Generic type arguments.
            Type[] genericArgTypes = type.GetGenericArguments();
            genericArgs = new TypeName[genericArgTypes.Length];
            for (int i = 0; i < genericArgTypes.Length; i++)
            {
                genericArgs[i] = new(genericArgTypes[i]);
            }

            // Main name.
            string fullName = type.Name;
            while (type.DeclaringType != null)
            {
                type = type.DeclaringType;
                fullName = type.Name + '+' + fullName;
            }
            name = fullName;
        }
    }
}