using System;
using Rusty.Serialization.Core.Codecs;
using Rusty.Serialization.Core.Nodes;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON serializer/deserializer back-end.
    /// </summary>
    public class JsonCodec : Codec
    {
        /* Public methods. */
        public override string Serialize(NodeTree node, Settings prettyPrint)
        {
            throw new NotImplementedException();
        }

        public override NodeTree Parse(string serialized)
        {
            throw new NotImplementedException();
        }
    }
}