using System;
using System.Collections.Generic;
using System.Text;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// Represents a type name.
    /// </summary>
    public readonly struct TypeName
    {
        /* Fields. */
        private readonly AliasRegistry context;
        private readonly Type type;

        private readonly string nameSpace;
        private readonly string name;
        private readonly TypeName[] genericArgs;

        /* Constructors. */
        public TypeName(Type type, AliasRegistry context)
        {
            // Store type & context.
            this.type = type;
            this.context = context;

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
            System.Console.WriteLine("INSTANTIATING " + name + " " + ToString());
        }

        public TypeName(string typeName, AliasRegistry context)
        {
            if (typeName == "")
            {
                this.context = context;
                type = null;
                nameSpace = "";
                name = "";
                genericArgs = [];
                return;
            }

            // Parse type.
            this.context = context;
            System.Console.WriteLine("Trying to resolve type typeName " + typeName);
            type = Type.GetType(typeName);
            System.Console.WriteLine($"The result was {(type != null ? type : "null")}");

            // Figure out namespace, main name and generic args parts.
            int namespaceIndex = -1;
            int genericsIndex = -1;

            int genericDepth = 0;
            for (int i = 0; i < typeName.Length; i++)
            {
                if (typeName[i] == '.' && genericsIndex == -1)
                    namespaceIndex = i;
                if (typeName[i] == '[')
                {
                    if (genericDepth == 0)
                        genericsIndex = i;
                    genericDepth++;
                }
                if (typeName[i] == ']')
                    genericDepth--;
            }

            // Namespace.
            if (namespaceIndex >= 0)
            {
                nameSpace = typeName.Substring(0, namespaceIndex);
                typeName = typeName.Substring(namespaceIndex + 1);
            }
            else
                nameSpace = "";

            // Generic arguments.
            if (genericsIndex >= 0 && genericsIndex + 2 < typeName.Length)
            {
                string generics = typeName.Substring(genericsIndex + 2);
                typeName = typeName.Substring(0, genericsIndex);
                genericArgs = ParseGenericArgs(generics, context);
            }
            else
                genericArgs = [];

            // Type name.
            this.name = typeName;
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

            // Apply alias.
            if (context.Has(type))
            {
                System.Console.WriteLine(context);
                System.Console.WriteLine("REPLACING NAME " + sb + " WITH " + context.Get(type));
                sb.Clear();
                sb.Append(context.Get(type));
            }

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