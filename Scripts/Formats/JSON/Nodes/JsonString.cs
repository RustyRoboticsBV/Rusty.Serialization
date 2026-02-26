namespace Rusty.Serialization.JSON
{
    public class JsonString : JsonNode
    {
        public string value;

        public JsonString(string value)
        {
            this.value = value;
        }
    }
}