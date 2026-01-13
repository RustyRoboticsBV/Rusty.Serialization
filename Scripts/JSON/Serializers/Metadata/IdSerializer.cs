using System;
using System.Globalization;
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.JSON
{
    /// <summary>
    /// A JSON ID serializer.
    /// </summary>
    public class IdSerializer : Serializer<IdNode>
    {
        /* Public methods. */
        public override string Serialize(IdNode node, ISerializerScheme scheme)
        {
            return '{' + $"\"id\":\"{node.Name}\",\"value\":{scheme.Serialize(node.Value)}" + '}';
        }

        public override IdNode Parse(string text, ISerializerScheme scheme)
        {
            return (IdNode)scheme.ParseAsNode(text);
        }
    }
}