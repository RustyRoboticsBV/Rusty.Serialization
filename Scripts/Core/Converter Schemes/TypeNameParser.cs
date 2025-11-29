using System;
using System.Collections.Generic;
using System.Text;

namespace Rusty.Serialization.Core.Converters
{
    /// <summary>
    /// A type name parser utility.
    /// </summary>
    internal static class TypeNameParser
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
                    + $"\n- Name: \"{Name}\""
                    + $"\n- GenericArgs: {genericArgs}"
                    + $"\n- ArraySuffix: \"{ArraySuffix}\"";
            }
        }

        /* Public methods. */
        public static void Test()
        {
            var examples = new string[]
            {
            "System.Collections.Generic.List`1[System.Int32][,][]",
            "System.Collections.Generic.List`1[System.Int32]",
            "System.Int32[]",
            "System.Int32",
            "MyCustomType`2+MyNestedType`1[System.Char, System.Int32, System.Single]"
            };

            foreach (var example in examples)
            {
                var parsed = Parse(example);
                Console.WriteLine(parsed);
            }
        }

        public static TypeNameParts Parse(string typeName)
        {
            var result = new TypeNameParts();
            result.OriginalName = typeName;
            int pos = 0;
            int length = typeName.Length;

            // Step 1: Extract the type name (up to first bracket)
            StringBuilder nameBuilder = new StringBuilder();
            while (pos < length && typeName[pos] != '[')
            {
                nameBuilder.Append(typeName[pos]);
                pos++;
            }

            string fullName = nameBuilder.ToString();

            // Step 1: Split namespace and name
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

            // Step 2: Parse brackets with depth counter
            var arraySuffixBuilder = new StringBuilder();
            while (pos < length && typeName[pos] == '[')
            {
                int startBracket = pos;
                int depth = 0;
                bool found = false;
                pos--; // We'll increment in loop

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

                if (!found) throw new ArgumentException("Mismatched brackets in type name.");
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