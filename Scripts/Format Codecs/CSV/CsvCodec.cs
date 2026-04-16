using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.CSV
{
    /// <summary>
    /// A CSV serializer/deserializer back-end.
    /// </summary>
    public class CsvCodec : FormatCodec
    {
        /* Private properties. */
        private CsvSerializer Serializer { get; } = new CsvSerializer();

        /* Public methods. */
        public override string Serialize(SyntaxTree node)
        {
            return Serializer.Serialize(node, Settings);
        }

        public override SyntaxTree Parse(string serialized)
        {
            return CsvParser.Parse(serialized);
        }
    }
}