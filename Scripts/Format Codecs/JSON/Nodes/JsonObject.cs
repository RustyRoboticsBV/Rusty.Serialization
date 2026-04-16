using System.Collections.Generic;

namespace Rusty.Serialization.JSON
{
    public class JsonObject : JsonNode
    {
        public List<string> keys = new List<string>();
        public List<JsonNode> values = new List<JsonNode>();

        public int Count => System.Math.Min(keys.Count, values.Count);

        public void Add(string key, JsonNode value)
        {
            keys.Add(key);
            values.Add(value);
        }
    }
}