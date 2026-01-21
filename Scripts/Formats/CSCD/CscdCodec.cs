using System;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD serializer/deserializer back-end.
    /// </summary>
    public class CscdCodec : Codec
    {
        /* Fields. */
        private CscdSerializer serializer = new CscdSerializer();
        private CscdLexer lexer = new CscdLexer();
        private CscdParser parser = new CscdParser();

        /* Public methods. */
        public override string Serialize(NodeTree tree, bool prettyPrint)
        {
            return serializer.Serialize(tree, prettyPrint);
        }

        public override NodeTree Parse(string serialized)
        {
            lexer.ResetCursor();
            return parser.Parse(serialized, lexer);
        }
    }
}