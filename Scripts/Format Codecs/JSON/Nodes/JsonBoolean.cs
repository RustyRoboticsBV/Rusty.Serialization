namespace Rusty.Serialization.JSON
{
    public class JsonBoolean : JsonNode
    {
        public bool value;

        public JsonBoolean(bool value)
        {
            this.value = value;
        }
    }
}