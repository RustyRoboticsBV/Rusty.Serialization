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
            System.Console.WriteLine(typeName);

            if (typeName == "")
            {
                this.context = context;
                type = null;
                nameSpace = "";
                name = "";
                genericArgs = [];
                return;
            }

            // Figure out namespace, main name, generic args and array suffix parts.
            string copy = typeName;
            int namespaceIndex = -1;
            int genericsIndex = -1;
            int genericsLength = 0;
            int arrayIndex = -1;

            int bracketDepth = 0;
            int bracketStart = 0;
            bool isGenericArgs = false;
            for (int i = 0; i < copy.Length; i++)
            {
                if (copy[i] == '.' && bracketDepth == 0)
                    namespaceIndex = i;
                else if (copy[i] == '[')
                {
                    if (bracketDepth == 0)
                        genericsIndex = i;
                    bracketDepth++;
                }
                else if (copy[i] == ']')
                {
                    bracketDepth--;
                    if (bracketDepth == 0)
                    {
                        if (!isGenericArgs || i < copy.Length - 1)
                            arrayIndex = i + 1;
                        else
                            genericsLength = i - genericsIndex - 1;
                        break;
                    }
                }
                else if (bracketDepth > 0)
                {
                    if (!isGenericArgs && (copy[i] >= 'a' && copy[i] <= 'z' || copy[i] >= 'A' && copy[i] <= 'Z'
                        || copy[i] == '_' || copy[i] == '`' || copy[i] == '.'))
                    {
                        isGenericArgs = true;
                        genericsIndex = bracketStart;
                    }
                }
            }

            // Namespace.
            if (namespaceIndex >= 0)
            {
                nameSpace = copy.Substring(0, namespaceIndex);
                copy = copy.Substring(namespaceIndex + 1);
            }
            else
                nameSpace = "";

            // Generic arguments.
            if (genericsIndex >= 0 && genericsIndex + 2 < copy.Length)
            {
                string generics = copy.Substring(genericsIndex + 2, 2);
                copy = copy.Substring(0, genericsIndex);
                genericArgs = ParseGenericArgs(generics, context);
            }
            else
                genericArgs = [];

            // Array suffix.
            if (arrayIndex >= 0)
                arraySuffix = copy.Substring(arrayIndex);

            // Type name.
            name = copy;

            // Get type.
            System.Console.WriteLine(typeName + "   ns:" + nameSpace + " n:" + name + " ga:" + genericArgs.Length);
            type = Type.GetType(ToString());
            if (type == null)
                throw new Exception("Bad type name: " + ToString());

            // Parse type.
            this.context = context;
        }

        /* Conversion operators. */
        public static implicit operator Type(TypeName tn) => tn.type;
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

            return sb.ToString();
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