#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
#define NET_JSON
#endif

using System;

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
    }
}