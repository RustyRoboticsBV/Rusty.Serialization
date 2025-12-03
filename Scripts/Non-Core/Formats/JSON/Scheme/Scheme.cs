using System;
using System.Text.Json;
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

        /* Public methods. */
        public string Serialize(INode node)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = PrettyPrint,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            throw new NotImplementedException();
        }

        public INode Parse(string serialized)
        {
            throw new NotImplementedException();
        }
    }
}