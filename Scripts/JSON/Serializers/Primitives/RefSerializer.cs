using System;
using System.Globalization;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON reference serializer.
    /// </summary>
    public class RefSerializer : Serializer<RefNode>
    {
        /* Public methods. */
        public override string Serialize(RefNode node, ISerializerScheme scheme)
        {
            return '{' + $" $ref = \"{node.ID}\" " + '}';
        }

        public override RefNode Parse(string text, ISerializerScheme scheme)
        {
            return (RefNode)scheme.ParseAsNode(text);
        }
    }
}