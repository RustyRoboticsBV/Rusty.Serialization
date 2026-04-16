using System.Collections.Generic;

namespace Rusty.Serialization.JSON
{
    public class JsonArray : JsonNode
    {
        public List<JsonNode> values = new List<JsonNode>();

        public int Count => values.Count;

        public void Add(JsonNode value)
        {
            values.Add(value);
        }
    }
}