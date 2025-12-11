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

        /* Private types. */
        /// <summary>
        /// A type name parser utility.
        /// </summary>
        private static class TypeNameParser
        {
            /* Public types. */
            /// <summary>
            /// A parser result.
            /// </summary>
            public class TypeNameParts
            {
                public string OriginalName { get; set; } = "";
                public string Namespace { get; set; } = "";
                public string Name { get; set; } = "";
                public List<string> GenericArgs { get; set; } = new List<string>();
                public string ArraySuffix { get; set; } = "";

                public override string ToString()
                {
                    StringBuilder genericArgs = new();
                    for (int i = 0; i < GenericArgs.Count; i++)
                    {
                        if (i > 0)
                            genericArgs.Append(", ");
                        genericArgs.Append("\"" + GenericArgs[i] + "\"");
                    }

                    return $"{OriginalName}"
                        + $"\n- Namespace: \"{Namespace}\""
                        + $"\n- index: \"{Name}\""
                        + $"\n- GenericArgs: {genericArgs}"
                        + $"\n- ArraySuffix: \"{ArraySuffix}\"";
                }
            }

            /* Public methods. */
            public static TypeNameParts Parse(string typeName)
            {
                var result = new TypeNameParts();
                result.OriginalName = typeName;
                int pos = 0;
                int length = typeName.Length;

                // Step 1: Extract the type name (up to first bracket).
                StringBuilder nameBuilder = new StringBuilder();
                while (pos < length && typeName[pos] != '[')
                {
                    nameBuilder.Append(typeName[pos]);
                    pos++;
                }

                string fullName = nameBuilder.ToString();

                // Step 2: Split namespace and name.
                int lastDot = fullName.LastIndexOf('.');
                if (lastDot >= 0)
                {
                    result.Namespace = fullName.Substring(0, lastDot);
                    result.Name = fullName.Substring(lastDot + 1);
                }
                else
                {
                    result.Name = fullName;
                }

                // Step 3: Parse brackets with depth counter.
                var arraySuffixBuilder = new StringBuilder();
                while (pos < length && typeName[pos] == '[')
                {
                    int startBracket = pos;
                    int depth = 0;
                    bool found = false;
                    pos--;

                    for (int i = startBracket; i < length; i++)
                    {
                        char c = typeName[i];
                        if (c == '[') depth++;
                        else if (c == ']') depth--;

                        if (depth == 0)
                        {
                            string content = typeName.Substring(startBracket + 1, i - startBracket - 1);

                            // Determine if generic args or array
                            if (ContainsLetter(content))
                            {
                                // Generic args
                                result.GenericArgs = SplitGenericArgs(content);
                            }
                            else
                            {
                                // Array suffix
                                arraySuffixBuilder.Append(typeName.Substring(startBracket, i - startBracket + 1));
                            }

                            pos = i + 1;
                            found = true;
                            break;
                        }
                    }

                    if (!found) throw new ArgumentException("Mismatched brackets in type index.");
                }

                result.ArraySuffix = arraySuffixBuilder.ToString();
                return result;
            }

            /* Private methods. */
            private static bool ContainsLetter(string s)
            {
                foreach (char c in s)
                {
                    if (char.IsLetter(c) || c == '_') return true;
                }
                return false;
            }

            private static List<string> SplitGenericArgs(string args)
            {
                var result = new List<string>();
                int depth = 0;
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < args.Length; i++)
                {
                    char c = args[i];
                    if (c == '[') depth++;
                    else if (c == ']') depth--;

                    if (c == ',' && depth == 0)
                    {
                        result.Add(sb.ToString().Trim());
                        sb.Clear();
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }

                if (sb.Length > 0)
                    result.Add(sb.ToString().Trim());

                return result;
            }
        }
    }
}