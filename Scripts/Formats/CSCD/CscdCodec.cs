using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.CSCD
{
    /// <summary>
    /// A CSCD serializer/deserializer back-end.
    /// </summary>
    public class CscdCodec : FormatCodec
    {
        /* Fields. */
        private CscdSerializer serializer = new CscdSerializer();
        private CscdLexer lexer = new CscdLexer();
        private CscdParser parser = new CscdParser();

        /* Public methods. */
        public override string Serialize(SyntaxTree tree, Settings settings)
        {
            return serializer.Serialize(tree, settings);
        }

        public override SyntaxTree Parse(string serialized)
        {
            lexer.ResetCursor();
            SyntaxTree tree = parser.Parse(serialized, lexer);
#if UNITY_5_3_OR_NEWER
            UnityEngine.Debug.Log(tree);
#endif
            return tree;
        }
    }
}