using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.CSV
{
    /// <summary>
    /// A CSV serializer/deserializer back-end.
    /// </summary>
    public class CsvCodec : Codec
    {
        /* Private properties. */
        private CsvSerializer Serializer { get; } = new CsvSerializer();

        /* Public methods. */
        public override string Serialize(SyntaxTree node, Settings settings)
        {
            return Serializer.Serialize(node, settings);
        }

        public override SyntaxTree Parse(string serialized)
        {
            return CsvParser.Parse(serialized);
        }
    }
}