#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
#define NET_JSON
#endif

#if !NET_JSON && !UNITY_5_3_OR_NEWER
using System;
#endif
using System.Text;


#if NET_JSON
using System.Text.Json;
#endif

#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif

using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public abstract class JsonSerializer<T> : Serializer<T>
        where T : INode
    {
        /* Public properties. */
        public abstract string Tag { get; }

        /* Private properties. */
#if NET_JSON
        private JsonSerializerOptions Options { get; } = new();
#endif

        /* Protected methods. */
        /// <summary>
        /// Serialize a JSON node.
        /// </summary>
        protected string NodeToText(JsonNode node, ISerializerScheme scheme)
        {
#if NET_JSON
            Options.WriteIndented = scheme.PrettyPrint;
            return JsonSerializer.Serialize(node, Options);
#elif UNITY_5_3_OR_NEWER
            return JsonUtility.ToJson(node, scheme.PrettyPrint);
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// Deserialize a string of JSON to a JSON node representation.
        /// </summary>
        protected U TextToNode<U>(string json)
            where U : JsonNode
        {
#if NET_JSON
            return JsonSerializer.Deserialize<U>(json);
#elif UNITY_5_3_OR_NEWER
            return JsonUtility.FromJson<U>(json);
#else
            throw new NotImplementedException();
#endif
        }

        protected static void OpenCollection(StringBuilder sb, char chr)
        {
            if (sb.Length > 0)
                sb.Clear();
            sb.Append(chr);
        }

        protected static void AddItem(StringBuilder sb, string name, string value, bool quoteValue, bool prettyPrint, string tab)
        {
            bool mustIndent = prettyPrint ? true : false;

            // Process value for pretty-printing.
            if (prettyPrint)
                value = value.Replace("\n", "\n" + tab);

            // Add comma if necessary.
            char lastChar = sb.Length > 0 ? sb[sb.Length - 1] : '\0';
            char nextChar = value.Length > 0 ? value[0] : '\0';
            if (sb.Length > 0 && (lastChar != '{' && nextChar != '{') && (lastChar != '[' && nextChar != '['))
                sb.Append(',');

            // Add linebreak.
            if (sb.Length > 0 && prettyPrint && lastChar != '\n' && nextChar != '\n')
                sb.Append('\n');

            // Add indentation.
            if (prettyPrint)
                sb.Append(tab);

            if (name != "")
            {
                // Add name.
                sb.Append('"');
                sb.Append(name);
                sb.Append('"');

                // Add separator.
                if (prettyPrint)
                    sb.Append(' ');
                sb.Append(':');
                if (prettyPrint)
                    sb.Append(' ');
            }

            // Add value.
            if (quoteValue)
                sb.Append('"');
            sb.Append(value);
            if (quoteValue)
                sb.Append('"');
        }

        protected static void CloseCollection(StringBuilder sb, char chr, bool prettyPrint)
        {
            if (prettyPrint)
            {
                char lastChar = sb.Length > 0 ? sb[sb.Length - 1] : '\0';
                if ((lastChar == '[' && chr == ']')
                    || (lastChar == '{' && chr == '}')
                    || (lastChar == '(' && chr == ')')
                    || lastChar == chr)
                {
                    sb.Append(' ');
                }
                else
                    sb.Append('\n');
            }
            sb.Append(chr);
        }
    }
}