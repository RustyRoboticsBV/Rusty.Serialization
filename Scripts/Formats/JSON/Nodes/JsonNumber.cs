namespace Rusty.Serialization.JSON
{
    public class JsonNumber : JsonNode
    {
        public double value;

        public JsonNumber(double value)
        {
            this.value = value;
        }
    }
}