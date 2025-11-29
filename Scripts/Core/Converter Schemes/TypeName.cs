using System;
using System.Collections.Generic;
using System.Text;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// Represents a type name.
    /// </summary>
    public struct TypeName
    {
        /* Fields. */
        private readonly AliasRegistry context;
        private readonly Type type;

        private string nameSpace;
        private string name;
        private TypeName[] genericArgs;
        private string arraySuffix;

        /* Constructors. */
        public TypeName(Type type, AliasRegistry context)
        {
            this.context = context;
            this.type = type;
            ParseType(type);
        }

        public TypeName(string typeName, AliasRegistry context)
        {
            var result = TypeNameParser.Parse(typeName);

            nameSpace = result.Namespace;
            name = result.Name;
            genericArgs = new TypeName[result.GenericArgs.Count];
            for (int i = 0; i < genericArgs.Length; i++)
            {
                genericArgs[i] = new(result.GenericArgs[i], null);
            }
            arraySuffix = result.ArraySuffix;

            // Parse type.
            this.context = context;
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

        public Type ToType() => Type.GetType(ToString());

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
                genericArgs = [];
                return;
            }

            // Handle generic type parameters.
            if (type.IsGenericParameter)
            {
                nameSpace = "";
                name = type.Name;
                genericArgs = [];
                return;
            }

            // Namespace.
            nameSpace = type.Namespace ?? "";

            // Generic type arguments.
            Type[] genericArgTypes = type.GetGenericArguments();
            genericArgs = new TypeName[genericArgTypes.Length];
            for (int i = 0; i < genericArgTypes.Length; i++)
            {
                genericArgs[i] = new(genericArgTypes[i], context);
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

        private static TypeName[] ParseGenericArgs(string str, AliasRegistry context)
        {
            if (str.StartsWith('[') && str.EndsWith(']'))
                str = str.Substring(1, str.Length - 2);

            int depth = 0;
            int termStart = 0;
            List<string> terms = new();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '[')
                    depth++;
                else if (str[i] == ']')
                    depth--;
                else if (str[i] == ',' && depth == 0)
                {
                    terms.Add(str.Substring(termStart, i));
                    termStart = i + 1;
                }
            }
            terms.Add(str.Substring(termStart));

            TypeName[] names = new TypeName[terms.Count];
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = new TypeName(terms[i], context);
            }

            return names;
        }
    }
}