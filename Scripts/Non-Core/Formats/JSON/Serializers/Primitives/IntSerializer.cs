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
    public class IntSerializer : Serializer<IntNode>
    {
        /* Public properties. */
        public string Tag => "int";

        /* Public methods. */
        public override string Serialize(IntNode node, ISerializerScheme scheme)
        {
            // Create serializable representation.
            JsonPrimitive<decimal> json = new(Tag, node.Value);

            // Serialize.
#if NET_JSON
            JsonSerializerOptions options = new();
            options.WriteIndented = scheme.PrettyPrint;
            return JsonSerializer.Serialize(json, options);
#elif UNITY_5_3_OR_NEWER
            return JsonUtility.ToJson(json, scheme.PrettyPrint);
#endif
        }

        public override IntNode Parse(string serialized, ISerializerScheme scheme)
        {
            // Don't allow empty strings.
            if (string.IsNullOrWhiteSpace(serialized))
                throw new ArgumentException("String is null or empty.");

            // Deserialize.
#if NET_JSON
            var json = JsonSerializer.Deserialize<JsonPrimitive<decimal>>(serialized);
            return new(json.value);
#elif UNITY_5_3_OR_NEWER
            var json = JsonUtility.FromJson<JsonPrimitive<decimal>>(serialized);
            return new(json.value);
#endif
        }
    }
}