using System;

namespace Rusty.Serialization.Serializers.JSON
{
    /// <summary>
    /// A representation of a JSON node that can be serialized and deserialized easily.
    /// </summary>
    [Serializable]
    public class JsonNode
    {
    }

    [Serializable]
    public class JsonSnippet : JsonNode
    {
        public string text;

        public JsonSnippet(string text)
        {
            this.text = text;
        }
    }

    /// <summary>
    /// A representation of a JSON node that can be serialized and deserialized easily.
    /// </summary>
    [Serializable]
    public class JsonPrimitive<T> : JsonNode
    {
        public string type;
        public T value;

        public JsonPrimitive(string type, T value) : base()
        {
            this.type = type;
            this.value = value;
        }
    }

    /// <summary>
    /// A representation of a JSON node that can be serialized and deserialized easily.
    /// </summary>
    [Serializable]
    public class JsonArray : JsonNode
    {
        public string type;
        public JsonNode[] value;

        public JsonArray(string type, JsonNode[] value) : base()
        {
            this.type = type;
            this.value = value;
        }
    }

    /// <summary>
    /// A representation of a JSON node that can be serialized and deserialized easily.
    /// </summary>
    [Serializable]
    public class JsonKeyValuePair<T, U> : JsonNode
    {
        public T key;
        public U value;

        public JsonKeyValuePair(T key, U value) : base()
        {
            this.key = key;
            this.value = value;
        }
    }

    /// <summary>
    /// A representation of a JSON node that can be serialized and deserialized easily.
    /// </summary>
    [Serializable]
    public class JsonDictionary<T, U> : JsonNode
    {
        public JsonKeyValuePair<T, U>[] pairs;

        public JsonDictionary(JsonKeyValuePair<T, U>[] pairs) : base()
        {
            this.pairs = pairs;
        }
    }

    /// <summary>
    /// A representation of a JSON node that can be serialized and deserialized easily.
    /// </summary>
    [Serializable]
    public class JsonType : JsonNode
    {
        public string type;
        public string name;
        public JsonNode value;

        public JsonType(string type, string name, JsonNode value) : base()
        {
            this.type = type;
            this.name = name;
            this.value = value;
        }
    }
}