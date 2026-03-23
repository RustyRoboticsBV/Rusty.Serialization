using System;
using System.Text;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON serializer/deserializer back-end.
    /// </summary>
    public class JsonCodec : FormatCodec
    {
        /* Private properties. */
        private JsonSerializer serializer { get; } = new JsonSerializer();
        private JsonParser parser { get; } = new JsonParser();
        private JsonLexer lexer { get; } = new JsonLexer();

        /* Public methods. */
        public override string Serialize(SyntaxTree tree, Settings settings)
        {
            return serializer.Serialize(tree, settings);
        }

        public override SyntaxTree Parse(string serialized)
        {
            TextSpan span = serialized;
            lexer.ResetCursor();
            return parser.Parse(span, lexer);
        }
    }
}