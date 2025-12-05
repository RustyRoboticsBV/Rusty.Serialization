using System;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    public class NullSerializer : JsonSerializer<NullNode>
    {
        /* Public properties. */
        public override string Tag => "null";

        /* Public methods. */
        public override string Serialize(NullNode node, ISerializerScheme scheme)
        {
            return "null";
        }

        public override NullNode Parse(string serialized, ISerializerScheme scheme)
        {
            if (serialized == "null")
                return new();
            else
                throw new Exception("Not null.");
        }
    }
}