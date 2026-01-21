using System;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.XML
{
    /// <summary>
    /// A XML serializer/deserializer back-end.
    /// </summary>
    public class XmlCodec : Codec
    {
        /* Public methods. */
        public override string Serialize(NodeTree node, bool prettyPrint)
        {
            throw new NotImplementedException();
        }

        public override NodeTree Parse(string serialized)
        {
            throw new NotImplementedException();
        }
    }
}