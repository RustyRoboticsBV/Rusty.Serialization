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
        public override string Serialize(NodeTree tree, Settings settings)
        {
            return serializer.Serialize(tree, settings);
        }

        public override NodeTree Parse(string serialized)
        {
            lexer.ResetCursor();
            NodeTree tree = parser.Parse(serialized, lexer);
#if UNITY_5_3_OR_NEWER
            UnityEngine.Debug.Log(tree);
#endif
            return tree;
        }
    }
}