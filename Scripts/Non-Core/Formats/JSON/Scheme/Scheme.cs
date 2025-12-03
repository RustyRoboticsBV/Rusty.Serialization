using System;
#if !UNITY_5_3_OR_NEWER
using System.Text.Json;
#endif
using Rusty.Serialization.Core.Nodes;
using Rusty.Serialization.Core.Serializers;

namespace Rusty.Serialization.Serializers.JSON
{
    /// <summary>
    /// A JSON serialization scheme.
    /// </summary>
    public class Scheme : ISerializerScheme
    {
        /* Public properties. */
        public bool PrettyPrint { get; set; }
        public string Tab { get; set; } = "  ";

        /* Private properties. */
        private IntSerializer Int { get; } = new();

        /* Public methods. */
        public string Serialize(INode node)
        {
            switch (node)
            {
                case IntNode i:
                    return Int.Serialize(node, this);
                default:
                    // TODO: remove this.
                    return "";
                    throw new ArgumentException($"Unknown node type '{node.GetType()}'.");
            }
            throw new NotImplementedException();
        }

        public INode Parse(string serialized)
        {
            throw new NotImplementedException();
        }
    }
}