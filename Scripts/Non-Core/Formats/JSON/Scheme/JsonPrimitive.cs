using System;

namespace Rusty.Serialization.Serializers.JSON
{
    /// <summary>
    /// A representation of a JSON node that can be serialized and deserialized easily.
    /// </summary>
    [Serializable]
    public struct JsonPrimitive<T>
    {
        public string type;
        public T value;

        public JsonPrimitive(string type, T value)
        {
            this.type = type;
            this.value = value;
        }
    }
}